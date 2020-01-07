using System.Collections.Generic;
using System.Web.Http;

namespace WebApiPractice.Controllers
{
    [RoutePrefix("IPAddress")]
    public class IPAddressController : ApiController
    {
        const string initialClientIP = "initialClientIP";
        const string forwardedClientIP = "forwardedClientIP";

        // GET: api/Colours
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            var (forwardedClientIPValue, initialClientIPValue) = GetInitialAndForwardedIPAddresses();

            return new string[] { $"{nameof(initialClientIP)}:{initialClientIPValue}", $"{nameof(forwardedClientIP)}:{forwardedClientIPValue}" };
        }

        // GET: api/Colours/5
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpGet, Route("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Colours
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Colours/5
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpPut, Route("value")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Colours/5
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpDelete, Route("{id}")]
        public void Delete(int id)
        {
        }

        // Private methods
        private (string, string) GetInitialAndForwardedIPAddresses()
        {
            Request.Properties.TryGetValue(forwardedClientIP, out var forwardedClientIPValue);
            Request.Properties.TryGetValue(initialClientIP, out var initialClientIPValue);

            return (forwardedClientIPValue as string, initialClientIPValue as string);
        }
    }
}
