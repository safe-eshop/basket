using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Ascii;
using Basket.Api.Framework.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Basket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLogger("Basket.Api")
                .ConfigureMetricsWithDefaults((context, builder) =>
                {
                    builder.Report.ToConsole(options =>
                    {
                        options.FlushInterval = TimeSpan.FromSeconds(20);
                        options.MetricsOutputFormatter = new MetricsTextOutputFormatter();
                    });
                })
                .UseMetrics()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "HelloWorldApp";
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloArchiveGrain).Assembly).WithReferences())
                        .AddMemoryGrainStorage(name: "ArchiveStorage");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
