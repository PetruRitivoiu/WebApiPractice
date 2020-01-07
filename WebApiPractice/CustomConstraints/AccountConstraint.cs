using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace WebApiPractice.CustomConstraints
{
    public class AccountConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            object value;

            if(values.TryGetValue(parameterName, out value) && value != null)
            {
                var stringVal = value as string;
                if (!string.IsNullOrEmpty(stringVal)) 
                {
                    return IsValidAccount(stringVal);
                }
            }

            return false;
        }

        public static bool IsValidAccount(string sAccount)
        {
            return (!string.IsNullOrEmpty(sAccount) &&
                sAccount.StartsWith("1234") &&
                sAccount.Length > 5);
        }
    }
}