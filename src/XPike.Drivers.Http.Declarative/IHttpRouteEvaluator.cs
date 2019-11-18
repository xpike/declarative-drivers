using XPike.Contracts;

namespace XPike.Drivers.Http.Declarative
{
    public interface IHttpRouteEvaluator
    {
        EvaluatedHttpRoute Evaluate<TRequest>(HttpHostInfo host, HttpRouteInfo httpRoute, TRequest request)
            where TRequest : class, IContract;
    }
}