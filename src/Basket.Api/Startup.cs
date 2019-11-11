using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Basket.Api.Framework.Logging;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Basket.Api
{
    public class Startup
    {
        public const string PathBaseEnviromentVariable = "PATH_BASE";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string PathBase => Configuration[PathBaseEnviromentVariable];

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            {
                services.AddControllers().AddJsonOptions(x => { x.JsonSerializerOptions.IgnoreNullValues = true; });

                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo() {Title = "Basket.Api", Version = "v1"});
                    var basePath = AppContext.BaseDirectory;
                    var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                    var fileName = Path.GetFileName(assemblyName + ".xml");
                    c.IncludeXmlComments(Path.Combine(basePath, fileName), includeControllerXmlComments: true);
                });


                services.AddCors(config =>
                {
                    var policy = new CorsPolicy();
                    policy.Headers.Add("*");
                    policy.Methods.Add("*");
                    policy.Origins.Add("*");
                    policy.SupportsCredentials = true;
                    config.AddPolicy("policy", policy);
                });

                services.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/json"});
                });
                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });
                services.AddHealthChecks()
                    .AddRedis(Configuration.GetConnectionString("BasketData"));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!string.IsNullOrEmpty(PathBase))
            {
                Log.Logger.Information($"Set BasePath {PathBase}");
                app.UsePathBase(PathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorLogging();
            }


            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    $"{(!string.IsNullOrEmpty(PathBase) ? PathBase : string.Empty)}/swagger/v1/swagger.json",
                    "Basket.Api");
                c.RoutePrefix = "swagger";
            });

            app.UseCors("policy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/ping", new HealthCheckOptions()
                {
                    Predicate = r => r.Name.Contains("self"),
                    ResponseWriter = PongWriteResponse,
                });
            });
        }

        private static Task PongWriteResponse(HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync("pong");
        }
    }
}