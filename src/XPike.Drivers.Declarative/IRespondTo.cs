using XPike.Contracts;

namespace XPike.Drivers.Declarative
{
    public interface IRespondTo<TRequest>
        : IContract
        where TRequest : class, IContract
    {
    }
}