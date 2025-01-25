using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HotelUp.Kitchen.Persistence.S3;

public static class Extensions
{
    private const string SectionName = "AWS:S3";
    
    public static IServiceCollection AddS3(this IServiceCollection services)
    {
        services.AddOptions<S3Options>()
            .BindConfiguration(SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<S3Options>>().Value;
        services.AddHostedService<S3BucketInitializer>();
        services.AddAWSService<IAmazonS3>(options: new AWSOptions()
        {
            Profile = options.Profile,
            Region = RegionEndpoint.GetBySystemName(options.Region)
        });
        return services;
    }
}