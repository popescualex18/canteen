using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using MMLib.SwaggerForOcelot.Middleware;
using Ocelot.Configuration.File;
using System.Text;

namespace Gateway.Utils
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddAuthentication(o => o.DefaultAuthenticateScheme = "Bearer")
            .AddJwtBearer("Bearer", config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromSeconds(0)
                };
            });
        }

        public static void Register(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSwaggerForOcelot(configuration, (options) =>
            {
                options.GenerateDocsForGatewayItSelf = false;
            });
            serviceCollection.AddSingleton<ISwaggerDownstreamInterceptor, PublishedDownstreamInterceptor>();

        }

        public static IServiceCollection ConfigureDownstreamHostAndPortPlaceholders(this IServiceCollection services, IConfiguration configuration)
        {
            services.PostConfigure<FileConfiguration>(fileConfiguration =>
            {
                foreach (var route in fileConfiguration.Routes)
                {
                    ConfigureRoute(route);
                }
            });

            return services;
        }
        private static void ConfigureRoute(FileRoute route)
        {
            foreach (var hostAndPort in route.DownstreamHostAndPorts)
            {
                //var placeHolder = hostAndPort.Host.ExtractSubstring(Constants.OptionStartDelimiter, Constants.OptionEndDelimiter);
                //if (!string.IsNullOrEmpty(placeHolder))
                //{
                //    var hostSettings = WebApiAddress.GetSettings(placeHolder);
                //    var index = hostSettings.Host.IndexOf("://");
                //    if (index != -1)
                //    {
                //        hostAndPort.Host = hostSettings.Host.Substring(index + 3);
                //        hostAndPort.Port = hostSettings.Port;
                //        continue;
                //    }
                //    hostAndPort.Host = hostSettings.Host;
                //    hostAndPort.Port = hostSettings.Port;
                //}
            }
        }
    }
}
