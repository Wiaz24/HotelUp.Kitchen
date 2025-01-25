using HotelUp.Kitchen.Shared.Auth;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace HotelUp.Kitchen.API.Swagger;

internal static class Extensions
{
    internal static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var oidcOptions = configuration.GetSection("Oidc").Get<OidcOptions>()
                          ?? throw new NullReferenceException("Oidc options are missing in appsettings.json");

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.UseInlineDefinitionsForEnums();
            options.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelUp.Kitchen", Version = "v1" });
            options.AddSecurityDefinition("oidc", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri(oidcOptions.MetadataAddress),
                Description = "OpenID Connect scheme",
                BearerFormat = "JWT",
                Extensions =
                {
                    { "x-tokenName", new OpenApiString("id_token") }
                }
            });
            var securityReqiurement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oidc"
                        }
                    },
                    new string[] { }
                }
            };
            options.AddSecurityRequirement(securityReqiurement);
        });
        return services;
    }

    internal static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        var oidcOptions = app.ApplicationServices.GetRequiredService<IOptions<OidcOptions>>().Value;

        app.UseSwagger(c => { c.RouteTemplate = $"api/kitchen/swagger/{{documentName}}/swagger.json"; });
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "HotelUp.Kitchen";
            c.SwaggerEndpoint($"/api/kitchen/swagger/v1/swagger.json", "API V1");
            c.RoutePrefix = $"api/kitchen/swagger";

            c.OAuthClientId(oidcOptions.ClientId);
            c.OAuthClientSecret(oidcOptions.ClientSecret);
            c.OAuthUsePkce();
        });
        return app;
    }
}