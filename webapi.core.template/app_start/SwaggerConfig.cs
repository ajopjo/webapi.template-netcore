using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace Webapi.Core.Template
{
    public static class SwaggerConfig
    {
        private const string SwaggerDocumentName = "<<document name here>>";
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerDocumentName, new Info
                {
                    Title = "<<name here>>",
                    Version = "v1",
                    Description = "<<description here>>"

                });
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "<<project xml documentation file name>>"));
                c.IgnoreObsoleteActions();
                

            });
            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            //swagger config here-> generate json
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";

            });

            //set up swagger UI
            app.UseSwaggerUI(c =>
            {
                // c.RoutePrefix = "help";//if you want a diffrent route name for swagger documentation
                c.SwaggerEndpoint($"/api-docs/{SwaggerDocumentName}/swagger.json", "<<Name here>>");

            });
            return app;
        }
    }
}
