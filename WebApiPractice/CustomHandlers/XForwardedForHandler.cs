using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebApiPractice.ExtensionMethods;

namespace WebApiPractice.CustomHandlers
{
    public class XForwardedForHandler : DelegatingHandler
    {
        //new style header: "Forwarded: by=<identifier>; for<identifier>; host=<host>; proto=<http|https>"
        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Forwarded
        const string fwdHeader = "Forwarded";

        //old stle, separate headers
        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-Proto
        const string xFwdProtoHeader = "X-Forwarded-Proto";
        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-Host
        const string xFwdHostHeader = "X-Forwarded-Host";
        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-For
        const string xFwdForHeader = "X-Forwarded-For";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //STEP 1: Global message-level logic that must be executed BEFORE the request
            //        is sent on to the action method
            var initialClientIP = string.Empty;
            if (request.Headers.Contains(fwdHeader))
            {
                var fwdHeaderValue = request.Headers.GetValues(fwdHeader).First();
                var forMatch = Regex.Match(fwdHeaderValue, @"for=\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}", RegexOptions.IgnoreCase);
                var ipMatch = Regex.Match(forMatch.Value, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

                initialClientIP = ipMatch.Value;
            }

            if (request.Headers.Contains(xFwdForHeader))
            {
                var fwdForHeaderValue = request.Headers.GetValues(xFwdForHeader).First();
                var ipMatch = Regex.Match(fwdForHeaderValue, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

                initialClientIP = ipMatch.Value;
            }

            string forwardedClientIP = request.GetClientIPAddress();
            if (!string.IsNullOrEmpty(initialClientIP))
            {
                request.Properties.Add(nameof(initialClientIP), initialClientIP);
            }

            if (!string.IsNullOrEmpty(forwardedClientIP))
            {
                request.Properties.Add(nameof(forwardedClientIP), forwardedClientIP);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}