using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotelUp.Kitchen.Persistence.S3;

public class S3BucketInitializer : IHostedService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3BucketInitializer> _logger;
    private readonly S3Options _options;

    public S3BucketInitializer(IAmazonS3 s3Client, ILogger<S3BucketInitializer> logger, 
        IOptions<S3Options> options)
    {
        _s3Client = s3Client;
        _logger = logger;
        _options = options.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var bucketName = _options.BucketName;
        try
        {
            if (!await DoesS3BucketExistAsync(bucketName, cancellationToken))
            {
                await CreateS3BucketAsync(bucketName, cancellationToken);
                var publicAccessBlockRequest = new PutPublicAccessBlockRequest
                {
                    BucketName = bucketName,
                    PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
                    {
                        BlockPublicAcls = false,
                        IgnorePublicAcls = false,
                        BlockPublicPolicy = false,
                        RestrictPublicBuckets = false
                    }
                };
                await _s3Client.PutPublicAccessBlockAsync(publicAccessBlockRequest, cancellationToken);
                
                var putPolicyRequest = new PutBucketPolicyRequest
                {
                    BucketName = bucketName,
                    Policy = @"{
                        ""Version"": ""2012-10-17"",
                        ""Statement"": [
                            {
                                ""Effect"": ""Allow"",
                                ""Principal"": ""*"",
                                ""Action"": ""s3:GetObject"",
                                ""Resource"": ""arn:aws:s3:::" + bucketName + @"/*""
                            }
                        ]
                    }"
                };    
                await _s3Client.PutBucketPolicyAsync(putPolicyRequest, cancellationToken);
                _logger.LogInformation("Bucket '{bucketName}' has been initialized.", bucketName);
            }
            else
            {
                _logger.LogInformation("Bucket '{bucketName}' already exists.", bucketName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while initializing the S3 bucket.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private async Task<bool> DoesS3BucketExistAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _s3Client.ListBucketsAsync(cancellationToken);
            return response.Buckets.Exists(
                b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot check if bucket '{bucketName}' exists.", bucketName);
            throw;
        }
    }
    
    private async Task CreateS3BucketAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            await _s3Client.PutBucketAsync(putBucketRequest, cancellationToken);
            _logger.LogInformation("Bucket '{bucketName}' has been created.", bucketName);
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Error while creating the bucket '{bucketName}': {Message}", 
                bucketName, ex.Message);
            throw;
        }
    }
}