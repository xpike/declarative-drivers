using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XPike.Settings;

namespace XPike.Drivers.Http.Declarative.Tests
{
    public class TestDriver
        : HttpDriverBase,
          ITestDriver
    {
        private readonly ISettings<HttpDriverSettings> _options;
        private readonly ISettings<TestDriverSettings> _testDriverOptions;

        public override HttpHostInfo HostInfo => new HttpHostInfo
                                                 {
                                                     BaseUri = new Uri("https://jsonplaceholder.typicode.com") //_testDriverOptions.Value.BaseUrl)
                                                 };

        public override HttpDriverSettings Settings => _options.Value;

        public TestDriver(ISettings<HttpDriverSettings> options, HttpClient client, ISettings<TestDriverSettings> testDriverOptions)
            : base(client)
        {
            _options = options;
            _testDriverOptions = testDriverOptions;
        }

        public Task<GetTodoResponse> GetTodoAsync(GetTodoQuery request, TimeSpan? timeout = null, CancellationToken? ct = null) =>
            base.GetHttpResponseAsync<GetTodoQuery, GetTodoResponse>(request, timeout, ct);

        //public Task<IHttpExchange<GetTodoQuery, GetTodoResponse>> GetExchangeAsync(GetTodoQuery request, TimeSpan? timeout = null, CancellationToken? ct = null) =>
        //    GetExchangeAsync<GetTodoQuery, GetTodoResponse>(request, timeout ?? TimeSpan.Parse(_testDriverOptions.Value.DefaultTimeout), ct);

        public Task<CreateTodoResponse> GetHttpResponseAsync(CreateTodoCommand request, TimeSpan? timeout = null, CancellationToken? ct = null) =>
            base.GetHttpResponseAsync<CreateTodoCommand, CreateTodoResponse>(request, timeout, ct);

        public Task<IHttpExchange<CreateTodoCommand, CreateTodoResponse>> GetHttpExchangeAsync(CreateTodoCommand request,
                                                                                               TimeSpan? timeout = null,
                                                                                               CancellationToken? ct = null) =>
            base.GetHttpExchangeAsync<CreateTodoCommand, CreateTodoResponse>(request, timeout ?? TimeSpan.Parse(_testDriverOptions.Value.DefaultTimeout), ct);
    }
}