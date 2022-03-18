using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASPPEER.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ASPPEER
{
    /// <summary>
    /// Class to configure the app on the startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration of app.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="configuration">Configuration of app.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Add methods and classes to the configuration.
        /// </summary>
        /// <param name="services">Services of app.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IUserRepository, UsersRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ASPPEER", Version = "v1"});
                var filePath = Path.Combine(AppContext.BaseDirectory, "MyComments.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Configuration of the app.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Web environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPPEER"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); });
            }
        }
    }