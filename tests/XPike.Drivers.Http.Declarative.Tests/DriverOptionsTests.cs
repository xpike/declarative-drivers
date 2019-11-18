using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XPike.Drivers.Http.Declarative.AspNetCore;
using Xunit;

namespace XPike.Drivers.Http.Declarative.Tests
{
    public class DriverOptionsTests
    {
        private IServiceProvider BuildProvider()
        {
            var config = new ConfigurationBuilder()
                         .AddInMemoryCollection(new Dictionary<string, string>
                                                {
                                                    {"XPike:Drivers:Http:Declarative:HttpDriverSettings:ProxyUrl", "http://localhost:8888"},
                                                    {"XPike:Drivers:Http:Declarative:HttpDriverSettings:DefaultTimeout", "00:00:59"}
                                                })
                         .Build();

            var collection = new ServiceCollection();
            collection.AddSingleton(config);
            collection.AddSingleton<IConfiguration>(config);

            collection.AddXPikeHttpDeclarativeDrivers();

            return collection.BuildServiceProvider();
        }
        
        [Fact]
        public void Test_DriverOptions()
        {
            var provider = BuildProvider();
            var config = provider.GetService<IOptions<HttpDriverSettings>>();

            Assert.NotNull(config);
            Assert.NotNull(config.Value);
            Assert.Equal("http://localhost:8888", config.Value.ProxyUrl);
            Assert.Equal("00:00:59", config.Value.DefaultTimeout);
        }
    }
}