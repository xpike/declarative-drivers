using System;
using System.Collections.Generic;
using System.Net;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative
{
    public class HttpExchange<TRequest, TResponse>
        : IHttpExchange<TRequest, TResponse>
        where TRequest : class, IRespondWith<TResponse>
        where TResponse : class, IRespondTo<TRequest>
    {
        public TRequest Request { get; set; }

        public TResponse Response { get; set; }

        public bool Transmitted { get; set; }

        public bool ResponseReceived { get; set; }

        public bool Successful { get; set; }

        public Exception Exception { get; set; }

        public EvaluatedHttpRoute Route { get; set; }

        public IReadOnlyDictionary<string, string> ResponseHeaders { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string RawResponse { get; set; }

        public TimeSpan? Elapsed { get; set; }
    }
}