using System;
using System.Collections.Generic;

namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// Specifies the details derived after fully evaluating HttpRouteAttribute.
    /// </summary>
    public class EvaluatedHttpRoute
        : HttpRouteInfo
    {
        /// <summary>
        /// The Host Info (its Base URL) to be used when connecting to the API.
        /// </summary>
        public HttpHostInfo HostInfo { get; set; }

        public string QualifiedRoute { get; set; }

        public IDictionary<string, string> QualifiedParameters { get; set; }

        public Uri QualifiedUri { get; set; }

        /// <summary>
        /// Creates a new Evaluated Route object using only the basic information from a HttpRouteInfo object.
        /// Used as the first step in converting the HttpRouteInfo from a HttpRouteAttribute into an EvaluatedHttpRoute.
        /// </summary>
        /// <param name="httpRoute"></param>
        public EvaluatedHttpRoute(HttpRouteInfo httpRoute)
        {
            Format = httpRoute.Format;
            Verb = httpRoute.Verb;
            Route = httpRoute.Route;
        }
    }
}