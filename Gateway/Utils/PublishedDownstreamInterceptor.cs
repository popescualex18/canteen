using Microsoft.VisualBasic;
using MMLib.SwaggerForOcelot.Configuration;
using MMLib.SwaggerForOcelot.Middleware;
using Ocelot.Configuration.File;
using Swashbuckle.AspNetCore.Swagger;
using System;
using RouteOptions = MMLib.SwaggerForOcelot.Configuration.RouteOptions;

namespace Gateway.Utils
{
    public class PublishedDownstreamInterceptor : ISwaggerDownstreamInterceptor
    {
        #region Private variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public PublishedDownstreamInterceptor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ISwaggerDownstreamInterceptor Implementation

        public bool DoDownstreamSwaggerEndpoint(HttpContext httpContext, string version, SwaggerEndPointOptions endPoint)
        {
            ConfigureSwaggerEndpoint(endPoint);

            return true;
        }

        #endregion

        #region Private Methods

        private void ConfigureSwaggerEndpoint(SwaggerEndPointOptions options)
        {
            var routes = _configuration.GetSection("Routes").Get<IList<FileRoute>>()?.ToList();

            foreach (var endpointConfig in options.Config)
            {
                var scheme = routes.First().DownstreamScheme;
                var hostAndPort = routes.First()!.DownstreamHostAndPorts;
               

            }
        }

        #endregion
    }
}
