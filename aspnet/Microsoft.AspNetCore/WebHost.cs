// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.WebHost
// Assembly: Microsoft.AspNetCore, Version=2.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 968FD0C4-FEC0-4F2F-91BC-F69CF8071DE5
// Assembly location: C:\Users\Denis\.nuget\packages\microsoft.aspnetcore\2.0.0\lib\netstandard2.0\Microsoft.AspNetCore.dll

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> and <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> with pre-configured defaults.
    /// </summary>
    public static class WebHost
    {
        /// <summary>
        /// Initializes and starts a new <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> with pre-configured defaults.
        /// See <see cref="M:Microsoft.AspNetCore.WebHost.CreateDefaultBuilder" /> for details.
        /// </summary>
        /// <param name="app">A delegate that handles requests to the application.</param>
        /// <returns>A started <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> that hosts the application.</returns>
        public static IWebHost Start(RequestDelegate app)
        {
            return WebHost.Start((string)null, app);
        }

        /// <summary>
        /// Initializes and starts a new <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> with pre-configured defaults.
        /// See <see cref="M:Microsoft.AspNetCore.WebHost.CreateDefaultBuilder" /> for details.
        /// </summary>
        /// <param name="url">The URL the hosted application will listen on.</param>
        /// <param name="app">A delegate that handles requests to the application.</param>
        /// <returns>A started <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> that hosts the application.</returns>
        public static IWebHost Start(string url, RequestDelegate app)
        {
            string name = app.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly.GetName().Name;
            return WebHost.StartWith(url, (Action<IServiceCollection>)null, (Action<IApplicationBuilder>)(appBuilder => appBuilder.Run(app)), name);
        }

        /// <summary>
        /// Initializes and starts a new <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> with pre-configured defaults.
        /// See <see cref="M:Microsoft.AspNetCore.WebHost.CreateDefaultBuilder" /> for details.
        /// </summary>
        /// <param name="routeBuilder">A delegate that configures the router for handling requests to the application.</param>
        /// <returns>A started <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> that hosts the application.</returns>
        public static IWebHost Start(Action<IRouteBuilder> routeBuilder)
        {
            return WebHost.Start((string)null, routeBuilder);
        }

        /// <summary>
        /// Initializes and starts a new <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> with pre-configured defaults.
        /// See <see cref="M:Microsoft.AspNetCore.WebHost.CreateDefaultBuilder" /> for details.
        /// </summary>
        /// <param name="url">The URL the hosted application will listen on.</param>
        /// <param name="routeBuilder">A delegate that configures the router for handling requests to the application.</param>
        /// <returns>A started <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> that hosts the application.</returns>
        public static IWebHost Start(string url, Action<IRouteBuilder> routeBuilder)
        {
            string name = routeBuilder.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly.GetName().Name;
            string url1 = url;
            Action<IApplicationBuilder> app = (Action<IApplicationBuilder>)(appBuilder => appBuilder.UseRouter(routeBuilder));
            string applicationName = name;
            return WebHost.StartWith(url1, (Action<IServiceCollection>)(services => services.AddRouting()), app, applicationName);
        }

        /// <summary>
        /// Initializes and starts a new <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> with pre-configured defaults.
        /// See <see cref="M:Microsoft.AspNetCore.WebHost.CreateDefaultBuilder" /> for details.
        /// </summary>
        /// <param name="app">The delegate that configures the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>A started <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> that hosts the application.</returns>
        public static IWebHost StartWith(Action<IApplicationBuilder> app)
        {
            return WebHost.StartWith((string)null, app);
        }

        /// <summary>
        /// Initializes and starts a new <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> with pre-configured defaults.
        /// See <see cref="M:Microsoft.AspNetCore.WebHost.CreateDefaultBuilder" /> for details.
        /// </summary>
        /// <param name="url">The URL the hosted application will listen on.</param>
        /// <param name="app">The delegate that configures the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>A started <see cref="T:Microsoft.AspNetCore.Hosting.IWebHost" /> that hosts the application.</returns>
        public static IWebHost StartWith(string url, Action<IApplicationBuilder> app)
        {
            return WebHost.StartWith(url, (Action<IServiceCollection>)null, app, (string)null);
        }

        private static IWebHost StartWith(string url, Action<IServiceCollection> configureServices, Action<IApplicationBuilder> app, string applicationName)
        {
            IWebHostBuilder defaultBuilder = WebHost.CreateDefaultBuilder();
            if (!string.IsNullOrEmpty(url))
                defaultBuilder.UseUrls(url);
            if (configureServices != null)
                defaultBuilder.ConfigureServices(configureServices);
            defaultBuilder.Configure(app);
            if (!string.IsNullOrEmpty(applicationName))
                defaultBuilder.UseSetting(WebHostDefaults.ApplicationKey, applicationName);
            IWebHost webHost = defaultBuilder.Build();
            webHost.Start();
            return webHost;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Hosting.WebHostBuilder" /> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="T:Microsoft.AspNetCore.Hosting.WebHostBuilder" />:
        ///     use Kestrel as the web server,
        ///     set the <see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.ContentRootPath" /> to the result of <see cref="M:System.IO.Directory.GetCurrentDirectory" />,
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from 'appsettings.json' and 'appsettings.[<see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName" />].json',
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from User Secrets when <see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName" /> is 'Development' using the entry assembly,
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from environment variables,
        ///     configures the <see cref="T:Microsoft.Extensions.Logging.ILoggerFactory" /> to log to the console and debug output,
        ///     enables IIS integration,
        ///     enables the ability for frameworks to bind their options to their default configuration sections,
        ///     and adds the developer exception page when <see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName" /> is 'Development'
        /// </remarks>
        /// <returns>The initialized <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</returns>
        public static IWebHostBuilder CreateDefaultBuilder()
        {
            return WebHost.CreateDefaultBuilder((string[])null);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Hosting.WebHostBuilder" /> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///   The following defaults are applied to the returned <see cref="T:Microsoft.AspNetCore.Hosting.WebHostBuilder" />:
        ///     use Kestrel as the web server,
        ///     set the <see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.ContentRootPath" /> to the result of <see cref="M:System.IO.Directory.GetCurrentDirectory" />,
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from 'appsettings.json' and 'appsettings.[<see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName" />].json',
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from User Secrets when <see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName" /> is 'Development' using the entry assembly,
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from environment variables,
        ///     load <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> from supplied command line args,
        ///     configures the <see cref="T:Microsoft.Extensions.Logging.ILoggerFactory" /> to log to the console and debug output,
        ///     enables IIS integration,
        ///     enables the ability for frameworks to bind their options to their default configuration sections,
        ///     and adds the developer exception page when <see cref="P:Microsoft.AspNetCore.Hosting.IHostingEnvironment.EnvironmentName" /> is 'Development'
        /// </remarks>
        /// <param name="args">The command line args.</param>
        /// <returns>The initialized <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</returns>
        public static IWebHostBuilder CreateDefaultBuilder(string[] args)
        {
            return new WebHostBuilder().UseKestrel().UseContentRoot(Directory.GetCurrentDirectory()).ConfigureAppConfiguration((Action<WebHostBuilderContext, IConfigurationBuilder>)((hostingContext, config) =>
            {
                IHostingEnvironment hostingEnvironment = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", true, true).AddJsonFile(string.Format("appsettings.{0}.json", (object)hostingEnvironment.EnvironmentName), true, true);
                if (hostingEnvironment.IsDevelopment())
                {
                    Assembly assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                    if (assembly != (Assembly)null)
                        config.AddUserSecrets(assembly, true);
                }
                config.AddEnvironmentVariables();
                if (args == null)
                    return;
                config.AddCommandLine(args);
            })).ConfigureLogging((Action<WebHostBuilderContext, ILoggingBuilder>)((hostingContext, logging) =>
            {
                logging.AddConfiguration((IConfiguration)hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
            }))/*.UseIISIntegration()*/.UseDefaultServiceProvider((Action<WebHostBuilderContext, ServiceProviderOptions>)((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment()));
        }
    }
}
