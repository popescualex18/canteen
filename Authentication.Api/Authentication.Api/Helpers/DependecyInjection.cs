using System.Text;
using Authentication.Core.Helpers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace AuthenticationApi.Helpers;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthorizationAndAuthentication(
        this IServiceCollection services,
        ApiOptions apiOptions)
    {
        var key = Encoding.ASCII.GetBytes(apiOptions.TokenOptions.Secret);
        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            //Same Secret key will be used while creating the token
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(token =>
        {
            token.RequireHttpsMetadata = false;
            token.SaveToken = true;
            token.TokenValidationParameters = tokenValidationParameter;

            token.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-7.0#built-in-jwt-authentication
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs/notifications"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }


}