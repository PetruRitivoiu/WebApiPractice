using System.Collections.Generic;
using System.Web.Http;
using WebApiPractice.AuthFilters;

namespace WebApiPractice.Controllers
{
    [RoutePrefix("BasicAuth")]
    public class BasicAuthController : ApiController
    {
        // GET: api/BasicAuth
        [OverrideAuthentication]
        [BasicAuth]
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { User.Identity.Name, User.Identity.AuthenticationType };
        }

        // GET: api/BasicAuth/5
        [OverrideAuthentication]
        [BasicAuth]
        [HttpGet, Route("id")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/BasicAuth
        [OverrideAuthentication]
        [BasicAuth]
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/BasicAuth/5
        [OverrideAuthentication]
        [BasicAuth]
        [HttpPut, Route("id")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BasicAuth/5
        [OverrideAuthentication]
        [BasicAuth]
        [HttpDelete, Route("id")]
        public void Delete(int id)
        {
        }
    }
}
