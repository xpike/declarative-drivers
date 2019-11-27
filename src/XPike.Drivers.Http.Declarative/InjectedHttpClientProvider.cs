using System.Net.Http;

namespace XPike.Drivers.Http.Declarative
{
    public class InjectedHttpClientProvider<TImplementation>
        : IInjectedHttpClientProvider<TImplementation>
        where TImplementation : IHttpDriver
    {
        public HttpClient Client { get; }

        public InjectedHttpClientProvider(HttpClient client)
        {
            Client = client;
        }
    }
}