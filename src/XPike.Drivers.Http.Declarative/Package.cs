using XPike.IoC;

namespace XPike.Drivers.Http.Declarative
{
    public class Package
    : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Settings.Package());

            dependencyCollection.RegisterSingleton<IHttpRouteEvaluator, HttpRouteEvaluator>();
            dependencyCollection.RegisterScoped(typeof(IInjectedHttpClientProvider<>), typeof(InjectedHttpClientProvider<>));
        }
    }
}