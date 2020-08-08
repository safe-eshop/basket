using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basket.Api.BasketOrleans;
using Basket.Api.Framework.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Orleans;
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
            services.AddControllers().AddMetrics();
            services.AddAppMetricsHealthPublishing();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.IncludeErrorDetails = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                };
            });
            services.AddAuthorization();
            services.AddCors();
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/json"});
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddHealthChecks();
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

            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = RequestLogging.EnrichFromRequest;
                opts.GetLevel = RequestLogging.ExcludeHealthChecks; // Use the custom level
            });
            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });
            app.UseAuthentication();
            app.UseAuthorization();
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
                
                endpoints.MapGet("/", async context =>
                {
                    IGrainFactory client = context.RequestServices.GetService<IGrainFactory>()!;
                    IHelloArchive grain = client.GetGrain<IHelloArchive>(0)!;
                    await grain.SayHello("Hello world");
                    await context.Response.WriteAsync("Keep refreshing your browser \n");
                    var res2 = await grain.GetGreetings();
                    await context.Response.WriteAsync(string.Join("\n", res2));
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