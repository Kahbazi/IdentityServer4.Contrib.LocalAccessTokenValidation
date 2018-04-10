using Microsoft.AspNetCore.Authentication;

namespace IdentityServer4.Contrib.LocalAccessTokenValidation
{
    public class LocalAccessTokenValidationOptions : AuthenticationSchemeOptions
    {
        public string ExpectedScope { get; set; }
    }
}