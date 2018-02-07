using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Mvc.Unit.Test.Framework.Routing.Mvc
{
    public class MockMvcRouteHandler : MvcRouteHandler, IRouter
    {
        private readonly IActionSelector _actionSelector;

        public MockMvcRouteHandler(IActionInvokerFactory actionInvokerFactory, IActionSelector actionSelector, DiagnosticSource diagnosticSource, ILoggerFactory loggerFactory) : base(actionInvokerFactory, actionSelector, diagnosticSource, loggerFactory)
        {
            _actionSelector = actionSelector;
        }

        public MockMvcRouteHandler(IActionInvokerFactory actionInvokerFactory, IActionSelector actionSelector, DiagnosticSource diagnosticSource, ILoggerFactory loggerFactory, IActionContextAccessor actionContextAccessor) : base(actionInvokerFactory, actionSelector, diagnosticSource, loggerFactory, actionContextAccessor)
        {
            _actionSelector = actionSelector;
        }

        public async Task RouteAsync(RouteContext context)
        {
            await base.RouteAsync(context);

            context.Handler = httpContext =>
            {
                var candidates = _actionSelector.SelectCandidates(context);
                var actionDescriptor = _actionSelector.SelectBestCandidate(context, candidates) as ControllerActionDescriptor;

                if (actionDescriptor == null)
                {
                    throw new NotImplementedException();
                }

                actionDescriptor.

                var routeData = httpContext.GetRouteData();
                return Task.CompletedTask;
            };

//            return Task.CompletedTask;
        }
    }
}