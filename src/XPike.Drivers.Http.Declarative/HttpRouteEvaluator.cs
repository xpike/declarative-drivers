using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using XPike.Contracts;

namespace XPike.Drivers.Http.Declarative
{
    /// <summary>
    /// Evaluates HttpRouteInfo
    /// </summary>
    public class HttpRouteEvaluator
        : IHttpRouteEvaluator
    {
        public EvaluatedHttpRoute Evaluate<TRequest>(HttpHostInfo host, HttpRouteInfo httpRoute, TRequest request)
            where TRequest : class, IContract
        {
            var parameters = GetParameters(request);
            var qualifiedRoute = PopulateRoute(httpRoute.Route, parameters);

            return new EvaluatedHttpRoute(httpRoute)
                   {
                       HostInfo = host,
                       QualifiedParameters = parameters,
                       QualifiedRoute = qualifiedRoute,
                       QualifiedUri = PopulateUrl(host.BaseUri, qualifiedRoute, httpRoute, parameters)
                   };
        }

        private IDictionary<string, string> GetParameters<TRequest>(TRequest request) =>
            JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(request));

        private Uri PopulateUrl(Uri hostUri, string qualifiedRoute, HttpRouteInfo httpRoute, IDictionary<string, string> parameters) =>
            new Uri(hostUri, $"{qualifiedRoute}{GenerateQueryString(httpRoute, parameters)}");

        private string GenerateQueryString(HttpRouteInfo httpRoute, IDictionary<string, string> parameters)
        {
            if (parameters == null)
                return string.Empty;

            switch (httpRoute.Verb)
            {
                case HttpVerb.Post:
                case HttpVerb.Put:
                case HttpVerb.Unknown:
                    return string.Empty;
            }

            var sb = new StringBuilder();
            var first = !httpRoute.Route.Contains("?");

            foreach (var item in parameters)
            {
                sb.Append(first ? "?" : "&");
                sb.Append(HttpUtility.UrlEncode(item.Key));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(item.Value));

                first = false;
            }

            return sb.ToString();
        }

        private string PopulateRoute(string route, IDictionary<string, string> parameters)
        {
            var used = new List<string>();

            foreach (var item in parameters)
            {
                if (route.Contains($"{{{item.Key}}}"))
                {
                    route = route.Replace($"{{{item.Key}}}", HttpUtility.UrlEncode(item.Value));
                    used.Add(item.Key);
                }
            }

            foreach (var item in used)
                parameters.Remove(item);

            return route;
        }
    }
}