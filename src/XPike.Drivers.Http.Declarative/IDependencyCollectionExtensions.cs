using XPike.IoC;

namespace XPike.Drivers.Http.Declarative
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeDeclarativeHttpDrivers(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Drivers.Http.Declarative.Package());
    }
}