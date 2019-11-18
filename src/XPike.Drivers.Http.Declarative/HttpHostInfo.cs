using System;

namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// Specifies the Host information (its Base URL) for connecting to an API over HTTP.
    /// Specific endpoint URLs (routes) are specified on each Contract using a HttpRouteAttribute.
    /// </summary>
    public class HttpHostInfo
    {
        /// <summary>
        /// The Base URI to use when connecting to the API.
        /// </summary>
        public Uri BaseUri { get; set; }
    }
}