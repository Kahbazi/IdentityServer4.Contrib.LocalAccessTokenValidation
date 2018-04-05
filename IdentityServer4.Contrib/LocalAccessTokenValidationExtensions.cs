using Microsoft.AspNetCore.Authentication;
using System;

namespace IdentityServer4.Contrib.LocalAccessTokenValidation
{
    public static class LocalAccessTokenValidationExtensions
    {
        public static AuthenticationBuilder AddLocalAccessTokenValidation(this AuthenticationBuilder builder)
            => builder.AddLocalAccessTokenValidation(LocalAccessTokenValidationDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddLocalAccessTokenValidation(this AuthenticationBuilder builder, Action<LocalAccessTokenValidationOptions> configureOptions)
            => builder.AddLocalAccessTokenValidation(LocalAccessTokenValidationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddLocalAccessTokenValidation(this AuthenticationBuilder builder, string authenticationScheme, Action<LocalAccessTokenValidationOptions> configureOptions)
            => builder.AddLocalAccessTokenValidation(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddLocalAccessTokenValidation(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<LocalAccessTokenValidationOptions> configureOptions)
        {
            return builder.AddScheme<LocalAccessTokenValidationOptions, LocalAccessTokenValidationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}