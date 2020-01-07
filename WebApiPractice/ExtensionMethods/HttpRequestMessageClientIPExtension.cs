using System.Net.Http;
using System.Web;

namespace WebApiPractice.ExtensionMethods
{
    public static class HttpRequestMessageClientIPExtension
    {
        public static string GetClientIPAddress(this HttpRequestMessage request)
        {
            string ip = null;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
                if (ctx != null)
                    ip = ctx.Request.UserHostAddress;
            }

            return ip;
        }
    }
}