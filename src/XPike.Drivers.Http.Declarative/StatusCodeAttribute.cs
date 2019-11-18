using System;
using System.Net;

namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// An attribute that can be applied to an Enum Member (aka "name", "value", "field")
    /// to indicate the HTTP Status Code that it corresponds to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StatusCodeAttribute
        : Attribute
    {
        /// <summary>
        /// The Status Code represented by this StatusCodeAttribute.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Creates a new StatusCodeAttribute.
        /// </summary>
        /// <param name="statusCode">The HttpStatusCode to be represented by the attribute.</param>
        public StatusCodeAttribute(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}