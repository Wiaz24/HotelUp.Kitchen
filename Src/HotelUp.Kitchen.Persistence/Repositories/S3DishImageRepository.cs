using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using FluentValidation;
using HotelUp.Kitchen.Persistence.Exceptions;
using HotelUp.Kitchen.Persistence.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotelUp.Kitchen.Persistence.Repositories;

public class S3DishImageRepository : IDishImageRepository
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Options _options;
    private readonly ILogger<S3DishImageRepository> _logger;
    private readonly TimeProvider _timeProvider;

    public S3DishImageRepository(IAmazonS3 s3Client, IOptionsSnapshot<S3Options> options, 
        ILogger<S3DishImageRepository> logger, TimeProvider timeProvider)
    {
        _s3Client = s3Client;
        _logger = logger;
        _timeProvider = timeProvider;
        _options = options.Value;
    }
    
    public static string GenerateKey(IFormFile image)
    {
        return $"dishes/{image.FileName}";
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        var validator = new ImageValidator();
        var validationResult = await validator.ValidateAsync(image);
        if (!validationResult.IsValid)
        {
            throw new InvalidDishImageFileException(validationResult.Errors);
        }
        var key = GenerateKey(image);
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            ContentType = image.ContentType,
            InputStream = image.OpenReadStream(),
            Metadata =
            {
                ["x-amz-meta-original-filename"] = image.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(image.FileName)
            }
        };
        var response = await _s3Client.PutObjectAsync(putObjectRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Failed to upload an image to S3. Status code: {StatusCode}.", response.HttpStatusCode);
            throw new DishImageUploadFailedException();
        }

        return $"https://s3.{_options.Region}.amazonaws.com/{_options.BucketName}/{key}";
    }

    private class ImageValidator : AbstractValidator<IFormFile>
    {
        public ImageValidator()
        {
            RuleFor(x => x.Length).LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("Image size should not exceed 5 MB.");
            RuleFor(x => x.ContentType).Must(x => x is "image/jpeg" or "image/jpg" or "image/png")
                .WithMessage("Only JPEG, JPG and PNG images are supported.");
        }
    }
}