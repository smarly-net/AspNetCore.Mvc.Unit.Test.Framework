using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace AspNetCore.Mvc.Unit.Test.Framework.Routing
{
    public class RoutingTest<TStartup> where TStartup : class
    {
        ServiceProvider serviceProvider;

        public RoutingTest<TStartup> Setup()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            IHostingEnvironment hostingEnvironment = new HostingEnvironment { EnvironmentName = "Development" };
            serviceCollection.AddSingleton(typeof(Microsoft.Extensions.Configuration.IConfiguration), typeof(Conf));

            var startupType = typeof(TStartup);

            if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
            {
                serviceCollection.AddSingleton(typeof(IStartup), startupType);
            }
            else
            {
                serviceCollection.AddSingleton(typeof(IStartup), sp => new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType, hostingEnvironment.EnvironmentName)));
            }

            serviceProvider = serviceCollection.BuildServiceProvider();

            return this;
        }

        public RoutingTest<TStartup> ShouldMap(string url)
        {
            this.Build();
            return this;
        }

        private RoutingTest<TStartup> Build()
        {
            var startup = serviceProvider.GetService<IStartup>();

            System.Diagnostics.Debug.Assert(startup != null);

            var applicationProvider = startup.ConfigureServices(new ServiceCollection());

            return this;
        }

        public void Equals<TController>(Action<TController> p) where TController : ControllerBase
        {
        }
    }

    public class AppBuilder : IApplicationBuilder
    {
        #region Implementation of IApplicationBuilder

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder New()
        {
            throw new NotImplementedException();
        }

        public RequestDelegate Build()
        {
            throw new NotImplementedException();
        }

        public IServiceProvider ApplicationServices { get; set; }
        public IFeatureCollection ServerFeatures { get; }
        public IDictionary<string, object> Properties { get; }

        #endregion
    }

    public class Conf : Microsoft.Extensions.Configuration.IConfiguration
    {
        #region Implementation of IConfiguration

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public string this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
