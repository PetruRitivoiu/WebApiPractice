using System.Collections.Generic;
using System.Web.Http;

namespace WebApiPractice.Controllers
{
    [RoutePrefix("CustomConstraints")]
    public class CustomConstraints : ApiController
    {
        [HttpGet, Route("")]
        // GET: api/Account
        public IEnumerable<string> GetAccounts()
        {
            return new string[] { "account-1", "account-2" };
        }

        [HttpGet, Route("{account:validAccount}")]
        // GET: api/Account/5
        public string GetAccountDetails(string account)
        {
            return account;
        }

        // POST: api/Account
        public void Post([FromBody]string value)
        {
        }

        [HttpPut, Route("{account:validAccount}")]
        // PUT: api/Account/5
        public void Put(string account, [FromBody]string value)
        {
        }

        [HttpDelete, Route("{account:validAccount}")]
        // DELETE: api/Account/5
        public void Delete(string account)
        {
        }
    }
}
