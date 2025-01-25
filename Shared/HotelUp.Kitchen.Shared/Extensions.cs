using HotelUp.Kitchen.Shared.Auth;
using HotelUp.Kitchen.Shared.Logging;
using HotelUp.Kitchen.Shared.Messaging;
using HotelUp.Kitchen.Shared.SystemsManager;
using HealthChecks.UI.Client;
using HotelUp.Kitchen.Shared.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace HotelUp.Kitchen.Shared;

public static class Extensions
{
    public static WebApplicationBuilder AddShared(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
        builder.AddCustomSystemsManagers();
        builder.Services.AddHealthChecks();
        builder.Services.AddAuth(builder.Configuration);
        builder.Services.AddHttpClient();
        builder.Services.AddMessaging();
        builder.Services.AddTransient<ExceptionMiddleware>();
        builder.AddCustomLogging();
        return builder;
    }

    public static IApplicationBuilder UseShared(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHealthChecks("/api/kitchen/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseAuth();
        return app;
    }
}