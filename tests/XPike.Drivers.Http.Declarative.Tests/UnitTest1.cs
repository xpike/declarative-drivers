using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using XPike.Settings.Basic;
using Xunit;

namespace XPike.Drivers.Http.Declarative.Tests
{
    public class UnitTest1
    {
        private ITestDriver CreateDriver() =>
            new TestDriver(new Settings<HttpDriverSettings>(typeof(HttpDriverSettings).FullName,
                                                            new HttpDriverSettings
                                                            {
                                                                DefaultTimeout = "00:00:15",
                                                                ProxyUrl = ""
                                                            }),
                           new InjectedHttpClientProvider<TestDriver>(new HttpClient()),
                           new Settings<TestDriverSettings>(typeof(TestDriverSettings).FullName,
                                                            new TestDriverSettings
                                                            {
                                                                BaseUrl = "https://jsonplaceholder.typicode.com",
                                                                DefaultTimeout = "00:00:05"
                                                            }));

        //private ITestDriver CreateDriver() =>
        //var driver = new TestDriver(new OptionsWrapper<DriverOptions>(new DriverOptions
        //                                                              {
        //                                                                  DefaultTimeout = "00:00:15",
        //                                                                  ProxyUrl = ""
        //                                                              }),
        //                            new HttpClient(),
        //                            new OptionsWrapper<TestDriverOptions>(new TestDriverOptions
        //                                                                  {
        //                                                                      BaseUrl = "https://jsonplaceholder.typicode.com",
        //                                                                      DefaultTimeout = "00:00:05"
        //                                                                  }));

        [Fact]
        public async Task TestGetAsync()
        {
            var driver = CreateDriver();

            var request = new GetTodoQuery
                          {
                              Id = 1
                          };

            var response = await driver.GetHttpExchangeAsync<GetTodoQuery, GetTodoResponse>(request);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.RawResponse);
            Assert.False(string.IsNullOrWhiteSpace(response.RawResponse));
            Assert.True(response.Transmitted);
            Assert.True(response.ResponseReceived);
            Assert.True(response.Successful);
            Assert.Null(response.Exception);
            Assert.Equal(request, response.Request);

            Assert.NotNull(response.Elapsed);
            Assert.True(response.Elapsed.Value > TimeSpan.Zero);

            Assert.NotNull(response.Response);
            Assert.NotNull(response.Route);
            Assert.NotNull(response.ResponseHeaders);
            Assert.True(response.ResponseHeaders.Any());

            var r = response.Response;
            Assert.False(r.Completed);
            Assert.Equal(1, r.Id);
            Assert.Equal(1, r.UserId);
            Assert.Equal("delectus aut autem", r.Title);

            request = new GetTodoQuery
                      {
                          Id = 4
                      };

            response = await driver.GetHttpExchangeAsync<GetTodoQuery, GetTodoResponse>(request);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.RawResponse);
            Assert.False(string.IsNullOrWhiteSpace(response.RawResponse));
            Assert.True(response.Transmitted);
            Assert.True(response.ResponseReceived);
            Assert.True(response.Successful);
            Assert.Null(response.Exception);
            Assert.Equal(request, response.Request);

            Assert.NotNull(response.Elapsed);
            Assert.True(response.Elapsed.Value > TimeSpan.Zero);

            Assert.NotNull(response.Response);
            Assert.NotNull(response.Route);
            Assert.NotNull(response.ResponseHeaders);
            Assert.True(response.ResponseHeaders.Any());

            r = response.Response;
            Assert.True(r.Completed);
            Assert.Equal(4, r.Id);
            Assert.Equal(1, r.UserId);
            Assert.Equal("et porro tempora", r.Title);
        }

        [Fact]
        public async Task TestPostAsync()
        {
            var driver = CreateDriver();

            var request = new CreateTodoCommand
                          {
                              UserId = 123,
                              Title = "Testing Things",
                              Body =
                                  "Space... The final frontier.  These are the voyages, of the starship Enterprise... it's continuing mission, to seek out strange new worlds, new life, and new civilizations.  To boldly go... where no man has gone before."
                          };

            var response = await driver.GetHttpExchangeAsync(request);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.RawResponse);
            Assert.False(string.IsNullOrWhiteSpace(response.RawResponse));
            Assert.True(response.Transmitted);
            Assert.True(response.ResponseReceived);
            Assert.True(response.Successful);
            Assert.Null(response.Exception);
            Assert.Equal(request, response.Request);

            Assert.NotNull(response.Elapsed);
            Assert.True(response.Elapsed.Value > TimeSpan.Zero);

            Assert.NotNull(response.Response);
            Assert.NotNull(response.Route);
            Assert.NotNull(response.ResponseHeaders);
            Assert.True(response.ResponseHeaders.Any());

            var r = response.Response;
            Assert.False(r.Completed);
            Assert.Equal(101, r.Id);
            Assert.Equal(123, r.UserId);
            Assert.Equal(request.Title, r.Title);
            Assert.Equal(request.Body, r.Body);
        }
    }
}