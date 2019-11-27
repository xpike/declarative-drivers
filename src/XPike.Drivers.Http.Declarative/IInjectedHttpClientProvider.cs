namespace XPike.Drivers.Http.Declarative
{
    public interface IInjectedHttpClientProvider<TImplementation>
        : IHttpClientProvider<TImplementation>
        where TImplementation : IHttpDriver
    {
    }
}