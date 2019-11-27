using System.Net.Http;

namespace XPike.Drivers.Http.Declarative
{
    public interface IHttpClientProvider<TImplementation>
        where TImplementation : IHttpDriver
    {
        HttpClient Client { get; }
    }
}