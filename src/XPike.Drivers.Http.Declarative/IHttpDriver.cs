using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative
{
    public interface IHttpDriver
        : IDriver
    {
        HttpHostInfo HostInfo { get; }

        Task<IHttpExchange<TRequest, TResponse>> GetHttpExchangeAsync<TRequest, TResponse>(TRequest request,
                                                                                           TimeSpan? timeout = null,
                                                                                           CancellationToken? ct = null,
                                                                                           IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>;

        Task<TResponse> GetHttpResponseAsync<TRequest, TResponse>(TRequest request,
                                                                  TimeSpan? timeout = null,
                                                                  CancellationToken? ct = null,
                                                                  IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>;
    }
}