using System.Collections.Generic;
using System.Web.Http;

namespace WebApiPractice.Controllers
{
    [RoutePrefix("AnyAuth")]
    public class AnyAuthController : ApiController
    {
        // GET: api/AnyAuth
        [Authorize]
        [HttpGet, Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { User.Identity.Name, User.Identity.AuthenticationType };
        }

        // GET: api/AnyAuth/5
        [Authorize]
        [HttpGet, Route("id")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AnyAuth
        [Authorize]
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AnyAuth/5
        [Authorize]
        [HttpPut, Route("")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AnyAuth/5
        [Authorize]
        [HttpDelete, Route("id")]
        public void Delete(int id)
        {
        }
    }
}
