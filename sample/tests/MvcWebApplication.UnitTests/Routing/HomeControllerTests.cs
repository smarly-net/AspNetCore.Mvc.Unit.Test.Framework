using AspNetCore.Mvc.Unit.Test.Framework.Routing;
using MvcWebApplication.Controllers;
using Xunit;

namespace MvcWebApplication.UnitTests.Routing
{
    public class HomeControllerTests
    {
        [Fact]
        public void Test1()
        {
            new RoutingTest<Startup>()
                .Setup()
                .ShouldMap("/Home/Index")
                .Equals<HomeController>(c => c.About());

        }
    }
}
