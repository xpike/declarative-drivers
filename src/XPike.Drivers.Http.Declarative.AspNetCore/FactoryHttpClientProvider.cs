using System.Net.Http;

namespace XPike.Drivers.Http.Declarative.AspNetCore
{
    public class FactoryHttpClientProvider<TImplementation>
        : IFactoryHttpClientProvider<TImplementation>
        where TImplementation : IHttpDriver
    {
        private readonly IHttpClientFactory _factory;

        public FactoryHttpClientProvider(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public HttpClient Client =>
            _factory.CreateClient(typeof(TImplementation).FullName);
    }
}