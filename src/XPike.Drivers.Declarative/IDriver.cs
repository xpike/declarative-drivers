using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XPike.Drivers.Declarative
{
    public interface IDriver
    {
        Task<IExchange<TRequest, TResponse>> GetExchangeAsync<TRequest, TResponse>(TRequest request,
                                                                                   TimeSpan? timeout = null,
                                                                                   CancellationToken? ct = null,
                                                                                   IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>;

        Task<TResponse> GetResponseAsync<TRequest, TResponse>(TRequest request,
                                                              TimeSpan? timeout = null,
                                                              CancellationToken? ct = null,
                                                              IDictionary<string, string> headers = null)
            where TRequest : class, IRespondWith<TResponse>
            where TResponse : class, IRespondTo<TRequest>;
    }
}