﻿using System;
using System.Text;
using AutoMapper;
using Digipolis.Web.Guidelines.Api.Configuration;
using Digipolis.Web.Guidelines.Api.Data;
using Digipolis.Web.Guidelines.Api.Logic;
using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Core.v3;

namespace Digipolis.Web.Guidelines.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(@"Configuration\appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($@"Configuration\appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC framework with defaulted settings applied.
            services.AddMvc()
                .AddMvcDefaults()
                .AddMvcOptions(x =>
                {
                    //Override default settings here
                    //...
                });

            //Add Api defaults AFTER AddMvc
            services.AddApiDefaults(x =>
            {
                //Enable global error handling and add error models for exceptions
                x.EnableGlobalErrorHandling<ApiExceptionHandler>();
                x.EnableVersioning();
            });

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen();
            services.ConfigureSwaggerGenDefaults().ConfigureSwaggerGen(x =>
            {
                //Override default settings here
                //...

                //Specify Api Versions here
                x.MultipleApiVersions(new Info[] { new Info
                {
                    //Add Inline version
                    Version = Settings.Versions.V1,
                    Title = "API V1",
                    Description = "Description for V1 of the API",
                    Contact = new Contact { Email = "info@digipolis.be", Name = "Digipolis", Url = "https://www.digipolis.be" },
                    TermsOfService = "https://www.digipolis.be/tos",
                    License = new License
                    {
                        Name = "My License",
                        Url = "https://www.digipolis.be/licensing"
                    },
                },
                //Add version through inheritence
                new InfoVersion2()});
            });

            services.AddEntityFramework();

            //Register Dependencies for example project
            services.AddScoped<IValueRepository, ValueRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IValueLogic, ValueLogic>();
            services.AddScoped<IFileLogic, FileLogic>();

            //Add AutoMapper
            services.AddAutoMapper();

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataContext dataContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Enable Api Defaults
            app.UseApiDefaults();

            //app.UseStaticFiles();
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();

            dataContext.Database.Migrate();
        }
    }
}
