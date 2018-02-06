using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvcApplication.Models;

namespace MvcApplication
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var env = serviceProvider.GetService<IHostingEnvironment>();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
//                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


//            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(StartupBase builder)
        {
        }


        /*        #region Implementation of IStartup

                public IServiceProvider ConfigureServices(IServiceCollection services)
                {
                    services.AddMvc();

                                return services.BuildServiceProvider();
                }

                public void Configure(IApplicationBuilder app)
                {
                    app.UseMvc(routes =>
                    {
                        routes.MapRoute(
                            name: "default",
                            template: "{controller=Home}/{action=Index}/{id?}");
                    });
                }

                #endregion*/
    }
}
