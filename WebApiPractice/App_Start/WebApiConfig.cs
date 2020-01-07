using System.Web.Http;
using System.Web.Http.Routing;
using WebApiPractice.AuthFilters;
using WebApiPractice.CustomConstraints;
using WebApiPractice.CustomHandlers;
using WebApiPractice.Filters;

namespace WebApiPractice
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SuppressHostPrincipal();

            // Web API configuration and services
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("validAccount", typeof(AccountConstraint));

            config.MessageHandlers.Add(new XHTTPMethodOverrideHandler());
            config.MessageHandlers.Add(new XForwardedForHandler());

            config.Filters.Add(new RouteTimeFilterAttribute());

            config.Filters.Add(new ApiKeyAuthAttribute());
            config.Filters.Add(new BasicAuthAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);

        }
    }
}
