using XPike.Contracts;

namespace XPike.Drivers.Declarative
{
    public interface IRespondWith<TResponse>
        : IContract
        where TResponse : class, IContract
    {
    }
}