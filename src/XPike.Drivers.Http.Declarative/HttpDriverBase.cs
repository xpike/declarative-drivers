using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative
{
    public abstract class HttpDriverBase
        : IHttpDriver
    {
        private static readonly IHttpRouteEvaluator _httpRouteEvaluator = new HttpRouteEvaluator();

        protected readonly HttpClient Client;

        protected Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public abstract HttpHostInfo HostInfo { get; }

        public abstract HttpDriverSettings Settings { get; }

        protected HttpDriverBase(HttpClient client)
        {
            Client = client;
            
            Client.DefaultRequestHeaders.Add("User-Agent", GetUserAgent());
            Client.Timeout = Timeout.InfiniteTimeSpan;
        }

        private string GetUserAgent() =>
            $"{GetType()} v{GetType().Assembly.GetName().Version} ({typeof(HttpDriverBase)} v{typeof(HttpDriverBase).Assembly.GetName().Version})";

        public async Task<TResponse> GetHttpResponseAsync<TRequest, TResponse>(TRequest request,
                                                                               TimeSpan? timeout = null,
                                                                               CancellationToken? ct = null,
                                                                               IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>
        {
            var exchange = await GetHttpExchangeAsync<TRequest, TResponse>(request, timeout, ct, headers);
            return exchange?.Successful ?? false ? exchange.Response : null;
        }

        private TimeSpan GetTimeout(TimeSpan? timeout = null) =>
            timeout ?? TimeSpan.Parse(Settings.DefaultTimeout);

        private CancellationToken GetCancellationToken(TimeSpan timeout, CancellationToken? ct = null)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);

            return ct == null ? cts.Token : CancellationTokenSource.CreateLinkedTokenSource(cts.Token, ct.Value).Token;
        }

        public async Task<IHttpExchange<TRequest, TResponse>> GetHttpExchangeAsync<TRequest, TResponse>(TRequest request,
                                                                                                        TimeSpan? timeout = null,
                                                                                                        CancellationToken? ct = null,
                                                                                                        IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>
        {
            var sw = Stopwatch.StartNew();
            timeout = GetTimeout(timeout);

            var route = _httpRouteEvaluator.Evaluate(HostInfo, request.GetHttpRouteInfo<TRequest, TResponse>(), request);
            var exchange = new HttpExchange<TRequest, TResponse>
                           {
                               Route = route,
                               Request = request
                           };

            try
            {
                MediaTypeFormatter formatter;

                switch (route.Format)
                {
                    case HttpFormat.Bson:
                        formatter = new BsonMediaTypeFormatter();
                        break;
                    case HttpFormat.Json:
                        formatter = new JsonMediaTypeFormatter();
                        break;
                    case HttpFormat.XML:
                        formatter = new XmlMediaTypeFormatter();
                        break;
                    case HttpFormat.FormEncoded:
                        formatter = new FormUrlEncodedMediaTypeFormatter();
                        break;
                    default:
                        throw new InvalidOperationException($"HTTP Format {route.Format} is not supported by {GetType()}!");
                }

                HttpMethod method;
                HttpContent content = null;

                switch (route.Verb)
                {
                    case HttpVerb.Get:
                        method = HttpMethod.Get;
                        break;
                    case HttpVerb.Delete:
                        method = HttpMethod.Delete;
                        break;
                    case HttpVerb.Post:
                    case HttpVerb.Put:
                        content = new ObjectContent<TRequest>(request, formatter);

                        if (headers?.Any() ?? false)
                            foreach (var header in headers)
                                content.Headers.Add(header.Key, header.Value);

                        switch (route.Verb)
                        {
                            case HttpVerb.Post:
                                method = HttpMethod.Post;
                                break;
                            case HttpVerb.Put:
                                method = HttpMethod.Post;
                                break;
                            default:
                                throw new InvalidOperationException("Follow the White Rabbit.");
                        }

                        break;
                    default:
                        throw new InvalidOperationException($"HTTP Verb {route.Verb} is not supported by {GetType()}!");
                }

                var message = new HttpRequestMessage(method, route.QualifiedUri);

                if (content != null)
                    message.Content = content;

                foreach (var header in Headers)
                    message.Headers.TryAddWithoutValidation(header.Key, header.Value);

                if (headers != null)
                    foreach (var header in headers)
                        message.Headers.TryAddWithoutValidation(header.Key, header.Value);

                foreach (var encoding in formatter.SupportedMediaTypes)
                    message.Headers.Accept.TryParseAdd(encoding.MediaType);

                var response = await Client.SendAsync(message, GetCancellationToken(GetTimeout(timeout), ct));

                exchange.Transmitted = true;

                if (response == null)
                {
                    exchange.Successful = false;
                    exchange.RawResponse = "Response was null.";
                    exchange.Elapsed = sw.Elapsed;

                    return exchange;
                }

                exchange.StatusCode = response.StatusCode;

                try
                {
                    try
                    {
                        exchange.ResponseHeaders = response.Headers.ToDictionary(x => x.Key, x => string.Join(";", x.Value));
                        exchange.RawResponse = await response.Content.ReadAsStringAsync();
                        exchange.ResponseReceived = true;
                    }
                    catch (Exception ex)
                    {
                        exchange.Exception = ex;
                    }

                    exchange.Response = await response.Content.ReadAsAsync<TResponse>();
                }
                catch (Exception ex)
                {
                    exchange.Exception = ex;
                }

                exchange.Successful = exchange.Response != null && (int) response.StatusCode >= 200 && (int) response.StatusCode < 300;
                exchange.Elapsed = sw.Elapsed;

                return exchange;
            }
            catch (Exception ex)
            {
                exchange.Successful = false;
                exchange.Exception = ex;
                exchange.Elapsed = sw.Elapsed;

                return exchange;
            }
        }

        public async Task<IExchange<TRequest, TResponse>> GetExchangeAsync<TRequest, TResponse>(TRequest request,
                                                                                                TimeSpan? timeout = null,
                                                                                                CancellationToken? ct = null,
                                                                                                IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest> =>
            await GetHttpExchangeAsync<TRequest, TResponse>(request, timeout, ct, headers);

        public Task<TResponse> GetResponseAsync<TRequest, TResponse>(TRequest request,
                                                                     TimeSpan? timeout = null,
                                                                     CancellationToken? ct = null,
                                                                     IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest> =>
            GetHttpResponseAsync<TRequest, TResponse>(request, timeout, ct, headers);
    }
}