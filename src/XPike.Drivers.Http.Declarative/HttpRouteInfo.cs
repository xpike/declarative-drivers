namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// Specifies the information about a Contract's Route.
    /// 
    /// Attach this to a Request Contract (a class that implements IRespondWith)
    /// by using a HttpRouteAttribute so that it can be used with HttpDriverBase.
    /// </summary>
    public class HttpRouteInfo
    {
        /// <summary>
        /// The HTTP Verb (eg GET, POST, PUT, DELETE) which should be used to access the endpoint.
        /// </summary>
        public HttpVerb Verb { get; set; }

        /// <summary>
        /// The Route format to use when accessing the endpoint.
        /// This should be a relative path, without the Base URL of the service.
        /// Replacement values can be specified by using {Variable} in the Route.
        /// These will be replaced with corresponding values from those at the root level of the Contract.
        /// Fields not specified here will be added to the QueryString for GET/DELETE requests, or posted in the Body for POST/PUT requests.
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// The serialization (or communication, for gRPC) format to use
        /// when transmitting the request over HTTP.
        /// </summary>
        public HttpFormat Format { get; set; }
    }
}