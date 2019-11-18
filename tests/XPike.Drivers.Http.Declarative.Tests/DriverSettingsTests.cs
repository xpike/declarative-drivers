using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPike.Configuration.Memory;
using XPike.Configuration.Microsoft.AspNetCore;
using XPike.Drivers.Http.Declarative.AspNetCore;
using XPike.Settings;
using XPike.Settings.AspNetCore;
using Xunit;

namespace XPike.Drivers.Http.Declarative.Tests
{
    public class DriverSettingsTests
    {
        public class Startup
        {
            public IConfiguration Configuration { get; }

            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddXPikeSettings();
                services.AddXPikeHttpDeclarativeDrivers();
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
            }
        }

        private IServiceProvider BuildProvider() =>
            new WebHostBuilder()
                .UseStartup<Startup>()
                .AddXPikeConfiguration(config =>
                                       {
                                           config.AddProvider(new MemoryConfigurationProvider(new Dictionary<string, string>
                                                                                              {
                                                                                                  {
                                                                                                      "XPike.Drivers.Http.Declarative.HttpDriverSettings::ProxyUrl",
                                                                                                      "http://localhost:8888"
                                                                                                  },
                                                                                                  {
                                                                                                      "XPike:Drivers:Http:Declarative:HttpDriverSettings:DefaultTimeout",
                                                                                                      "00:00:59"
                                                                                                  }
                                                                                              }));
                                       })
                .Build()
                .Services;
        
        [Fact]
        public void Test_DriverSettings()
        {
            var provider = BuildProvider();
            var config = provider.GetService<ISettings<HttpDriverSettings>>();

            Assert.NotNull(config);
            Assert.NotNull(config.Value);
            Assert.Equal("http://localhost:8888", config.Value.ProxyUrl);
            Assert.Equal("00:00:59", config.Value.DefaultTimeout);
        }
    }
}