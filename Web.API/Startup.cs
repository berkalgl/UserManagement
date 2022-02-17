using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using UserManagement.Domain;
using UserManagement.WebApi.Extensions;
using UserManagement.WebApi.Handlers;

namespace Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Register();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.API v1"));
            }

            applicationLifetime.ApplicationStarted.Register(OnApplicationStarted);
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context => ModuleActionExceptionHandler.HandleException(context));
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void OnApplicationStarted()
        {
            var loggerRepository = LogManager.GetRepository();
            if (loggerRepository == null || !LogManager.GetRepository().Configured)
            {
                XmlConfigurator.Configure();
            }

            try
            {
                //var bootstrap = new Bootstrapper();
                //GlobalConfiguration.Configuration.EnsureInitialized();
                //bootstrap.Initialize(GlobalConfiguration.Configuration);
            }
            catch (Exception ex)
            {
                var unhandledExceptionLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                unhandledExceptionLog.Fatal(string.Format(
                        "UnHandled Exception for Url {0}",
                        string.Empty),
                    ex);
            }

        }

    }
}
