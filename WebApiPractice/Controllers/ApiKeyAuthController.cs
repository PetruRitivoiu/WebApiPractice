using System.Collections.Generic;
using System.Web.Http;
using WebApiPractice.AuthFilters;

namespace WebApiPractice.Controllers
{
    [RoutePrefix("ApiKeyAuth")]
    public class ApiKeyAuthController : ApiController
    {
        // GET: api/ApiKeyAuth
        [OverrideAuthentication]
        [ApiKeyAuth]
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { User.Identity.Name, User.Identity.AuthenticationType };
        }

        // GET: api/ApiKeyAuth/5
        [OverrideAuthentication]
        [ApiKeyAuth]
        [HttpGet, Route("id")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ApiKeyAuth
        [OverrideAuthentication]
        [ApiKeyAuth]
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApiKeyAuth/5
        [OverrideAuthentication]
        [ApiKeyAuth]
        [HttpPut, Route("")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiKeyAuth/5
        [OverrideAuthentication]
        [ApiKeyAuth]
        [HttpDelete, Route("id")]
        public void Delete(int id)
        {
        }
    }
}
