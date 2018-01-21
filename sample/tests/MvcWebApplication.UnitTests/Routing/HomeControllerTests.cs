using AspNetCore.Mvc.Unit.Test.Framework.Routing;
using MvcApplication.Controllers;
using Xunit;

namespace MvcApplication.UnitTests.Routing
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
