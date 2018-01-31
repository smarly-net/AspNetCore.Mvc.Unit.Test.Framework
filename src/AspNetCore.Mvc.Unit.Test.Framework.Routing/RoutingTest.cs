using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Primitives;
using Moq;

namespace AspNetCore.Mvc.Unit.Test.Framework.Routing
{
    public class RoutingTest<TStartup> where TStartup : class
    {
//        ServiceProvider serviceProvider;
            IServiceCollection serviceCollection;

        public RoutingTest<TStartup> Setup()
        {

            Trace.Write("Some Red Message");

            serviceCollection = new ServiceCollection();
            IHostingEnvironment hostingEnvironment = new HostingEnvironment { EnvironmentName = "Development" };
            serviceCollection.AddSingleton(typeof(Microsoft.Extensions.Configuration.IConfiguration), typeof(Conf));
            serviceCollection.AddSingleton(typeof(IHostingEnvironment), sp => hostingEnvironment);
            serviceCollection.AddSingleton(typeof(IHostingEnvironment), sp => hostingEnvironment);

            serviceCollection.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            var listener = new DiagnosticListener("Microsoft.AspNetCore");
            serviceCollection.AddSingleton<DiagnosticListener>(listener);
            serviceCollection.AddSingleton<DiagnosticSource>(listener);

            ILoggerFactory f = new LoggerFactory(new []{ NullLoggerProvider.Instance });

            Mock<ILoggerFactory> logger = new Mock<ILoggerFactory>();
            serviceCollection.AddSingleton(typeof(ILoggerFactory), sp => f);

            var startupType = typeof(TStartup);

            if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
            {
                serviceCollection.AddSingleton(typeof(IStartup), startupType);
            }
            else
            {
                serviceCollection.AddSingleton(typeof(IStartup), sp => new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType, hostingEnvironment.EnvironmentName)));
            }

//            serviceProvider = serviceCollection.BuildServiceProvider();

            return this;
        }

        public static IServiceCollection Clone(IServiceCollection serviceCollection)
        {
            IServiceCollection clone = new ServiceCollection();
            foreach (var service in serviceCollection)
            {
                clone.Add(service);
            }
            return clone;
        }

        public RoutingTest<TStartup> ShouldMap(string url)
        {
            this.Build();
            return this;
        }

        private RoutingTest<TStartup> Build()
        {


            var startup = Clone(serviceCollection).BuildServiceProvider().GetService<IStartup>();

            System.Diagnostics.Debug.Assert(startup != null);

            var applicationProvider = startup.ConfigureServices(serviceCollection);

            ApplicationBuilderFactory f = new ApplicationBuilderFactory(applicationProvider);
            FeatureCollection featureCollection = new FeatureCollection();
            featureCollection.Set(new ServerAddressesFeature());
            var builder = f.CreateBuilder(featureCollection);

            startup.Configure(builder);

            return this;
        }

        public void Equals<TController>(Action<TController> p) where TController : ControllerBase
        {
        }
    }

    public class ServerAddressesFeature : IServerAddressesFeature {
        #region Implementation of IServerAddressesFeature

        public ICollection<string> Addresses { get; } = new List<string>();
        public bool PreferHostingUrls { get; set; }

        #endregion
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
