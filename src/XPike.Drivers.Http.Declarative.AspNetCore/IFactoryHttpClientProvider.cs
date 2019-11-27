namespace XPike.Drivers.Http.Declarative.AspNetCore
{
    public interface IFactoryHttpClientProvider<TImplementation>
        : IHttpClientProvider<TImplementation>
        where TImplementation : IHttpDriver
    {
    }
}