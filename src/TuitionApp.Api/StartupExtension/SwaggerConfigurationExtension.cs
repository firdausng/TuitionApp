using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuitionApp.Api.StartupExtension
{
    public static class SwaggerConfigurationExtension
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Tuition App API";
                    document.Info.Description = "An ASP.NET Core web API for Tuition App";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Firdaus Kamaruddin",
                        Email = string.Empty,
                        Url = "https://www.firdausng.com"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });
        }

        public static IApplicationBuilder UseCustomSwaggerConfig(this IApplicationBuilder app)
        {
            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
            return app;
        }
    }
}
