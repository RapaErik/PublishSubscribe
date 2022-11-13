using PublishSubscribe.Services;

namespace PublishSubscribe.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
               .AddSingleton<ISubscribeService, SubscribeService>();
        public static IServiceCollection AddBackgroundWorkerServices(this IServiceCollection services)
             => services.AddHostedService<BrokerService>();


        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services.AddHttpClient<ISenderService, SenderService>().ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

            });
            return services;
        }
    }
}
