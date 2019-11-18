using System;
using System.Threading;
using System.Threading.Tasks;
using XPike.Drivers.Declarative;

namespace XPike.Drivers.Http.Declarative
{
    public interface IDriveHttp
        : IHttpDriver
    {
    }

    /// <summary>
    /// You shouldn't *need* to use this interface, unless...
    /// You wanted to provide a type-specific override for GetHttpResponseAsync and GetHttpExchangeAsync.
    /// For "normal" driver purposes, you should define methods in your driver interface with use-case-specific names.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IDriveHttp<TRequest, TResponse>
        : IDriveHttp
        where TRequest : class, IRespondWith<TResponse>
        where TResponse : class, IRespondTo<TRequest>
    {
        Task<TResponse> GetHttpResponseAsync(TRequest request, TimeSpan? timeout = null, CancellationToken? ct = null);

        Task<IHttpExchange<TRequest, TResponse>> GetHttpExchangeAsync(TRequest request, TimeSpan? timeout = null, CancellationToken? ct = null);
    }
}