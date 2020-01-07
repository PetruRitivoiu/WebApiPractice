using System.Collections.Generic;
using System.Web.Http;

namespace WebApiPractice.Controllers
{
    [RoutePrefix("MethodOverride")]
    public class MethodOverrideController : ApiController
    {
        // GET: api/Values
        [OverrideAuthentication]
        [AllowAnonymous]
        [AcceptVerbs("GET", "VIEW", "HEAD"), Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Values/5
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpGet, Route("{id:int}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpPost, Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Values/5
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpPut, Route("{id:int}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Values/5
        [OverrideAuthentication]
        [AllowAnonymous]
        [HttpDelete, Route("{id:int}")]
        public void Delete(int id)
        {
        }
    }
}
