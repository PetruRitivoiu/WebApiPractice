using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebApiPractice.AuthFilters
{
    public class BasicAuthAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;
        public const string SupportedTokenScheme = "Basic";

        public bool SendChallenge { get; set; }

        public BasicAuthAttribute()
        {
            SendChallenge = true;
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            //Get the Authorization Header
            var authHeader = context.Request.Headers.Authorization;
            if (authHeader == null)
            {
                return;
            }

            //Check if the Authorization Header scheme is supported
            var authScheme = authHeader.Scheme;
            if (!authScheme.Equals(SupportedTokenScheme))
            {
                return;
            }

            //Retrieve credentials and check for null or empty
            var credentials = authHeader.Parameter;
            if (string.IsNullOrEmpty(credentials))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", context.Request);
                return;
            }

            //validate the credentials
            IPrincipal principal = await ValidateCredentials(credentials, cancellationToken);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", context.Request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            if (SendChallenge)
            {
                context.Result = new AddChallengeOnUnauthorizedResult(new AuthenticationHeaderValue(SupportedTokenScheme), context.Result);
            }

            return Task.FromResult(0);
        }

        private async Task<IPrincipal> ValidateCredentials(string credentials, CancellationToken cancellationToken)
        {
            var (subject, password) = ParseBasicAuthCredentials(credentials);

            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(password))
            {
                if (subject == "admin" && password == "admin")
                {
                    IList<Claim> claimCollection = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, subject),
                        new Claim(ClaimTypes.AuthenticationInstant, DateTime.UtcNow.ToString("o")),
                        new Claim("urn:MyCustomClaim", "my special value")
                    };

                    var identity = new ClaimsIdentity(claimCollection, SupportedTokenScheme);
                    var principal = new ClaimsPrincipal(identity);

                    return await Task.FromResult(principal);
                }
            }

            return null;
        }

        private Tuple<string, string> ParseBasicAuthCredentials(string credentials)
        {
            string subject = null;
            string password = null;

            var decodedCredential = Encoding.GetEncoding("utf-8").GetString(Convert.FromBase64String(credentials));
            if (string.IsNullOrEmpty(decodedCredential))
            {
                return new Tuple<string, string>(null, null);
            }

            if (decodedCredential.Contains(":"))
            {
                var decodedCredentialSplitArray = decodedCredential.Split(':');
                subject = decodedCredentialSplitArray[0];
                password = decodedCredentialSplitArray[1];
            }

            return new Tuple<string, string>(subject, password);
        }
    }
}