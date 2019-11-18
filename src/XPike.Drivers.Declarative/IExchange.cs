using System;
using System.Collections.Generic;

namespace XPike.Drivers.Declarative
{
    public interface IExchange<out TRequest, out TResponse>
        where TRequest : class, IRespondWith<TResponse>
        where TResponse : class, IRespondTo<TRequest>
    {
        TRequest Request { get; }

        TResponse Response { get; }

        bool Transmitted { get; }

        bool ResponseReceived { get; }

        bool Successful { get; }

        Exception Exception { get; }

        IReadOnlyDictionary<string, string> ResponseHeaders { get; }

        TimeSpan? Elapsed { get; }
    }
}