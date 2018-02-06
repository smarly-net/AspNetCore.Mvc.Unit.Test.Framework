using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.Mvc.Unit.Test.Framework.Routing;
using AspNetCore.Mvc.Unit.Test.Framework.Routing.Server;
using Microsoft.AspNetCore.Hosting;
using MvcApplication.Controllers;
using Xunit;

namespace MvcApplication.UnitTests.Routing
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange
            var webHostBuilder = Program.CreateWebHostBuilder(Array.Empty<string>())
                .UseContentRoot(Path.GetFullPath("../../../../../apps/MvcApplication"));

            var server = new TestServer(webHostBuilder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            new RoutingTest<Startup>()
                .Setup()
                .ShouldMap("/Home/Index")
                .Equals<HomeController>(c => c.About());

        }
    }
}
