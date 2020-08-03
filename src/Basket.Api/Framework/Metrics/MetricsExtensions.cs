using Microsoft.Extensions.DependencyInjection;

namespace Basket.Api.Framework.Metrics
{
    public static class MetricsExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            services.AddMetricsEndpoints();
            return services;
        }
    }
}