using System;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiPractice.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class RouteTimeFilterAttribute : ActionFilterAttribute
    {
        public const string Header = "X-API-Timer";
        public const string TimerPropertyName = "RouteTimerFilter_";
        public string TimerName;

        public RouteTimeFilterAttribute(string name = null)
        {
            TimerName = name;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var name = TimerName ?? actionContext.ActionDescriptor.ActionName;

            actionContext.Request.Properties[TimerPropertyName + name] = Stopwatch.StartNew();

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var name = TimerName ?? actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            var timer = (Stopwatch)actionExecutedContext.ActionContext.Request.Properties[TimerPropertyName + name];
            var time = timer.ElapsedMilliseconds;

            base.OnActionExecuted(actionExecutedContext);

            actionExecutedContext.ActionContext.Response.Headers.Add(Header, time + " msec");
        }

    }
}