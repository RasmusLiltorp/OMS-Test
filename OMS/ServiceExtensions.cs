using Microsoft.Extensions.DependencyInjection;

namespace OMS_Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddOmsServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.AddScoped<ApiService>();
            services.AddScoped<PIMApiService>();
            services.AddScoped<AnalyticsService>();
            services.AddScoped<DataService>();
            
            services.AddScoped<IEmailService, SmtpEmailService>();
            
            return services;
        }
    }
}