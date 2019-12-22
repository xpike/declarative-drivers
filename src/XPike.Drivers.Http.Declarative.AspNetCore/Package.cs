using XPike.IoC;

namespace XPike.Drivers.Http.Declarative.AspNetCore
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Drivers.Http.Declarative.Package());

            dependencyCollection.RegisterScoped(typeof(IInjectedHttpClientProvider<>), typeof(InjectedHttpClientProvider<>));
            dependencyCollection.RegisterScoped(typeof(IFactoryHttpClientProvider<>), typeof(FactoryHttpClientProvider<>));
            dependencyCollection.RegisterSingleton<IHttpRouteEvaluator, HttpRouteEvaluator>();

            dependencyCollection.RegisterScoped(typeof(IHttpClientProvider<>), typeof(FactoryHttpClientProvider<>));
        }
    }
}