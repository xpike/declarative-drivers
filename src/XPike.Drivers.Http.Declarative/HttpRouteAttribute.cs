using System;

namespace XPike.Drivers.Http.Declarative
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpRouteAttribute
        : Attribute
    {
        public HttpRouteInfo Route { get; set; }

        public HttpRouteAttribute(HttpVerb verb, string route, HttpFormat format)
        {
            Route = new HttpRouteInfo
                    {
                        Format = format,
                        Route = route,
                        Verb = verb
                    };
        }
    }
}