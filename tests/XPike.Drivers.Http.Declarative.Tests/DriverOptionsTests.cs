using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPike.Configuration;
using XPike.Configuration.Microsoft.AspNetCore;
using XPike.Drivers.Http.Declarative.AspNetCore;
using XPike.Settings.AspNetCore;
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

            collection.AddXPikeHttpDeclarativeDrivers()
                .AddXPikeSettings()
                .UseMicrosoftConfigurationForXPike();

            return collection.BuildServiceProvider();
        }
        
        [Fact]
        public void Test_DriverOptions()
        {
            var provider = BuildProvider();
            var config = provider.GetService<IConfig<HttpDriverSettings>>();

            Assert.NotNull(config);
            Assert.NotNull(config.CurrentValue);
            Assert.Equal("http://localhost:8888", config.CurrentValue.ProxyUrl);
            Assert.Equal("00:00:59", config.CurrentValue.DefaultTimeout);
        }
    }
}