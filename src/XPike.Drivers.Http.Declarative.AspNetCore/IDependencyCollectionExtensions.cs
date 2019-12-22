using XPike.IoC;

namespace XPike.Drivers.Http.Declarative.AspNetCore
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeAspNetCoreHttpDeclarativeDrivers(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Drivers.Http.Declarative.AspNetCore.Package());
    }
}