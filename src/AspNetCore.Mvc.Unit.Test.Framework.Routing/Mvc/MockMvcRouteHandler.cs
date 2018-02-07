using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IActionInvokerFactory _actionInvokerFactory;
        private readonly IActionSelector _actionSelector;
        private readonly IActionContextAccessor _actionContextAccessor;

        public MockMvcRouteHandler(IActionInvokerFactory actionInvokerFactory, IActionSelector actionSelector, DiagnosticSource diagnosticSource, ILoggerFactory loggerFactory) : this(actionInvokerFactory, actionSelector, diagnosticSource, loggerFactory, actionContextAccessor: null)
        {
        }

        public MockMvcRouteHandler(IActionInvokerFactory actionInvokerFactory, IActionSelector actionSelector, DiagnosticSource diagnosticSource, ILoggerFactory loggerFactory, IActionContextAccessor actionContextAccessor) : base(actionInvokerFactory, actionSelector, diagnosticSource, loggerFactory, actionContextAccessor)
        {
            _actionInvokerFactory = actionInvokerFactory;
            _actionSelector = actionSelector;
            _actionContextAccessor = actionContextAccessor;
        }

        async Task IRouter.RouteAsync(RouteContext context)
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

                var routeData = httpContext.GetRouteData();

                var actionContext = new ActionContext(context.HttpContext, routeData, actionDescriptor);
                if (_actionContextAccessor != null)
                {
                    _actionContextAccessor.ActionContext = actionContext;
                }

                var invoker = _actionInvokerFactory.CreateInvoker(actionContext);
                if (invoker == null)
                {
                    throw new NotImplementedException();
                }

                return invoker.InvokeAsync();



                return Task.CompletedTask;
            };

//            return Task.CompletedTask;
        }
    }
}