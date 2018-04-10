using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.LocalAccessTokenValidation
{
    public class LocalAccessTokenValidationHandler : AuthenticationHandler<LocalAccessTokenValidationOptions>
    {
        private readonly ITokenValidator _tokenValidator;

        public LocalAccessTokenValidationHandler(IOptionsMonitor<LocalAccessTokenValidationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITokenValidator tokenValidator)
            : base(options, logger, encoder, clock)
        {
            _tokenValidator = tokenValidator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string requestToken = null;

            string authorization = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.Fail("No Authorization Header is sent.");
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                requestToken = authorization.Substring("Bearer ".Length).Trim();
            }

            if (string.IsNullOrEmpty(requestToken))
            {
                return AuthenticateResult.Fail("No Access Token is sent.");
            }

            TokenValidationResult result = await _tokenValidator.ValidateAccessTokenAsync(requestToken, Options.ExpectedScope);

            if (result.IsError)
            {
                return AuthenticateResult.Fail(result.Error);
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(result.Claims, Scheme.Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            AuthenticationTicket authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            return AuthenticateResult.Success(authenticationTicket);
        }
    }
}