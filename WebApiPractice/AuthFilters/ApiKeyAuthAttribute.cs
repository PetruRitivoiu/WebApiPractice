using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebApiPractice.AuthFilters
{
    public class ApiKeyAuthAttribute : Attribute, IAuthenticationFilter
    {
        public const string XAPIKey = "X-API-Key";
        public const string SupportedScheme = "Bearer";
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            //Get the authorization header and the legacy x-api-key header
            var authorizationHeader = context.ActionContext.Request.Headers.Authorization;
            context.ActionContext.Request.Headers.TryGetValues(XAPIKey, out var xApiKeyHeaders);
            var xApiKeyHeader = xApiKeyHeaders?.FirstOrDefault();

            //Check authorization header parameter and authorization scheme
            var credentials =
                ((authorizationHeader?.Scheme == SupportedScheme) && !string.IsNullOrEmpty(authorizationHeader?.Parameter)) ?
                authorizationHeader?.Parameter :
                xApiKeyHeader;

            if (string.IsNullOrEmpty(credentials))
            {
                return;
            }

            //Validate the credentials and retrieve the account ID
            var principal = await ValidateCredentialsAsync(credentials, cancellationToken);

            //Add the principal to the context
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", context.Request);
            } else
            {
                context.Principal = principal;
            }
            
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //do nothing
            return Task.FromResult(0);
        }

        private async Task<IPrincipal> ValidateCredentialsAsync(string credentials, CancellationToken cancellationToken)
        {
            if (credentials.Length != 8)
            {
                return null;
            }

            var accountID = credentials.Take(3).ToString();

            IList<Claim> claimsCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credentials),
                new Claim("urn:ClientAccount", accountID),
                new Claim(ClaimTypes.AuthenticationInstant, DateTime.Now.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claimsCollection, SupportedScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return await Task.FromResult(claimsPrincipal);
        }
    }
}