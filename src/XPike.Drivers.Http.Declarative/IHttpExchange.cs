using System.Net;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative
{
    public interface IHttpExchange<out TRequest, out TResponse>
        : IExchange<TRequest, TResponse>
        where TRequest : class, IRespondWith<TResponse>
        where TResponse : class, IRespondTo<TRequest>
    {
        HttpStatusCode StatusCode { get; }

        string RawResponse { get; }

        EvaluatedHttpRoute Route { get; }
    }
}