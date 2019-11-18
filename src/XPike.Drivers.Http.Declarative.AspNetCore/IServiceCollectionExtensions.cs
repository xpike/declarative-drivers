using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XPike.Drivers.Http.Declarative.AspNetCore
{
    /// <summary>
    /// Extension methods for registering the xPike Declarative Drivers library with the DI provider.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds dependency registrations for the xPike Declarative Drivers library to an IServiceCollection.
        ///
        /// IOptions:
        /// - HttpDriverSettings
        ///
        /// Singletons:
        /// - IHttpRouteEvaluator = HttpRouteEvaluator
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddXPikeHttpDeclarativeDrivers(this IServiceCollection services)
        {
            services.AddOptions<HttpDriverSettings>()
                    .Configure<IConfiguration>((options, configuration) =>
                                                   configuration.GetSection(typeof(HttpDriverSettings).FullName.Replace(".", ":"))
                                                                .Bind(options));

            services.AddSingleton<IHttpRouteEvaluator, HttpRouteEvaluator>();

            return services;
        }

        public static IHttpClientBuilder AddHttpDeclarativeDriver<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class, IDriveHttp
            where TImplementation : class, TInterface =>
            services.AddHttpClient<TInterface, TImplementation>();

        public static IHttpClientBuilder AddHttpDeclarativeDriver<TInterface, TImplementation>(this IServiceCollection services,
                                                                                               string name)
            where TInterface : class, IDriveHttp
            where TImplementation : class, TInterface =>
            services.AddHttpClient<TInterface, TImplementation>(name);

        public static IHttpClientBuilder AddHttpDeclarativeDriver<TInterface, TImplementation>(this IServiceCollection services,
                                                                                               Action<IServiceProvider, HttpClient> configureAction)
            where TInterface : class, IDriveHttp
            where TImplementation : class, TInterface =>
            services.AddHttpClient<TInterface, TImplementation>(configureAction);

        public static IHttpClientBuilder AddHttpDeclarativeDriver<TInterface, TImplementation>(this IServiceCollection services,
                                                                                               string name,
                                                                                               Action<IServiceProvider, HttpClient> configureAction)
            where TInterface : class, IDriveHttp
            where TImplementation : class, TInterface =>
            services.AddHttpClient<TInterface, TImplementation>(name, configureAction);
    }
}