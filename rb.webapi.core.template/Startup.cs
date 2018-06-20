using System;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Webapi.Core.Template.Filters;

namespace Webapi.Core.Template
{
    public class Startup
    {
        public Autofac.IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
                    options.Filters.Add(typeof(ValidateModelAttribute));

                })//since web api core services are configured, use required features|pipelines
            .AddJsonFormatters()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddJsonOptions(options => { options.SerializerSettings.Formatting = Formatting.Indented; });

            //use swagger for API documentation - refer app_start folder
            services.AddSwaggerServices();

            //use Autofac as the dependency container. Refer app_start folder for extension methods
            ApplicationContainer = services.AddAutofacContainer();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            app.UseMvc().UseSwagger();
            applicationLifetime.ApplicationStopped.Register(() => { ApplicationContainer.Dispose(); });
        }
    }
}
