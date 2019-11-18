using System;
using System.Linq;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative
{
    // ReSharper disable once InconsistentNaming
    public static class IRespondWithExtensions
    {
        public static HttpRouteInfo GetHttpRouteInfo<TRequest, TResponse>(this TRequest request, bool throwIfMissing = true)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>
        {
            var route = (typeof(TRequest).GetCustomAttributes(typeof(HttpRouteAttribute), true).FirstOrDefault() as HttpRouteAttribute)?.Route;

            if (route == null && throwIfMissing)
                throw new InvalidOperationException($"No HttpRouteAttribute found on type {typeof(TRequest)}.");

            return route;
        }
    }
}