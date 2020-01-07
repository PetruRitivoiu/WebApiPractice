using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiPractice.CustomHandlers
{
    public class XHTTPMethodOverrideHandler : DelegatingHandler
    {
        const string xHttpHeaderName = "X-HTTP-Method-Override";
        readonly List<string> allowedMethods = new List<string> { "PUT", "PATCH", "DELETE", "HEAD", "VIEW" };

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //STEP 1: Global message-level logic that must be executed BEFORE the request
            //        is sent on to the action method

            if (request.Method != HttpMethod.Post)
            {
                return base.SendAsync(request, cancellationToken);
            } else
            {
                if (request.Headers.Contains(xHttpHeaderName))
                {
                    var xHttpHeaderValue = request.Headers.GetValues(xHttpHeaderName).FirstOrDefault();
                    if (allowedMethods.Contains(xHttpHeaderValue))
                    {
                        request.Method = new HttpMethod(xHttpHeaderValue);
                    } else
                    {
                        var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("X-HTTP-Method-Override header value must have on of the following values: { 'PUT', 'PATCH', 'DELETE', 'HEAD', 'VIEW' }")
                        };

                        return Task.FromResult(errorResponse);
                    }
                }
            }

            //STEP 2: Call the rest of the pipeline, all the way to a response message
            var response = base.SendAsync(request, cancellationToken);

            //STEP 3: Any global message-level logic that must be executed AFTER the request
            //        has executed, before the final HTTP response message


            //STEP 4: Return the final HTTP response
            return response;
        }
    }
}