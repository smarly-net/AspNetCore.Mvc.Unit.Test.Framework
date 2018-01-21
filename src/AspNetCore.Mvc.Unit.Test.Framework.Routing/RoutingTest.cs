using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
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
            ServiceCollection serviceCollection;
            TStartup startup;
        public RoutingTest<TStartup> Setup()
        {
            //setup our DI
            serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<TStartup>();
            serviceCollection.AddSingleton(typeof(Microsoft.Extensions.Configuration.IConfiguration), typeof(Conf));

            serviceProvider = serviceCollection.BuildServiceProvider();

            //do the actual work here
            startup = serviceProvider.GetService<TStartup>();

            return this;
        }

        public RoutingTest<TStartup> ShouldMap(string url)
        {
            this.Build();
            return this; 
        }

        private RoutingTest<TStartup> Build()
        {


            return this;
        }

        public void Equals<TController>(Action<TController> p) where TController : ControllerBase
        {
            startup.GetType().GetMethod("ConfigureServices").Invoke(startup, new[] { serviceCollection });
            AppBuilder app = new AppBuilder(){ApplicationServices = serviceProvider };

            app.UseMvc();

            startup.GetType().GetMethod("Configure").Invoke(startup, new[] { app });
        }
    }

    public class AppBuilder : IApplicationBuilder {
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
