using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InquiryService.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace InquiryService
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            AppConfiguration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ConnectionFactory>(new ConnectionFactory() { HostName = AppConfiguration["HostName"], UserName= AppConfiguration["UserName"], Password = AppConfiguration["Password"] });
            services.AddSingleton<RabbitClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/Events-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
