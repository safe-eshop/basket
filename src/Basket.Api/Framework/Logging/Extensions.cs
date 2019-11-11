using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace Basket.Api.Framework.Logging
{
 public static class Extensions
    {
        private static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);
            return model;
        }

        public static IHostBuilder UseLogger(this IHostBuilder hostBuilder, string applicationName = null)
        {
            return hostBuilder.UseSerilog(((context, configuration) =>
            {
                var serilogOptions = context.Configuration.GetOptions<SerilogOptions>("Serilog");
                var seqOptions = context.Configuration.GetOptions<SeqOptions>("Seq");
                if (!Enum.TryParse<LogEventLevel>(serilogOptions.MinimumLevel, true, out var level))
                {
                    level = LogEventLevel.Information;
                }

                var conf = configuration
                    .MinimumLevel.Is(level)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", applicationName)
                    .Enrich.WithDemystifiedStackTraces()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Enrich.WithThreadId()
                    .Enrich.WithExceptionDetails();

                conf.WriteTo.Async((logger) =>
                {
                    if (seqOptions.Enabled)
                    {
                        logger.Seq(seqOptions.Url ?? "http://localhost:5341");
                    }

                    if (serilogOptions.ConsoleEnabled)
                    {
                        if (serilogOptions.Format.ToLower() == "elasticsearch")
                        {
                            logger.Console(new ElasticsearchJsonFormatter());
                        }
                        else if (serilogOptions.Format.ToLower() == "compact")
                        {
                            logger.Console(new CompactJsonFormatter());
                        }
                        else if (serilogOptions.Format.ToLower() == "colored")
                        {
                            logger.Console(theme: AnsiConsoleTheme.Code);
                        }
                    }

                    logger.Trace();
                });
            }));
        }
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}