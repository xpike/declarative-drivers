using System;
using System.Threading;
using System.Threading.Tasks;

namespace XPike.Drivers.Http.Declarative.Tests
{
    public interface ITestDriver
        : //IDriveHttp<GetTodoQuery, GetTodoResponse>,
            IDriveHttp<CreateTodoCommand, CreateTodoResponse>
    {
        Task<GetTodoResponse> GetTodoAsync(GetTodoQuery request, TimeSpan? timeout = null, CancellationToken? ct = null);
    }
}