using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace Gateway.Middleware
{
    public class TrafficInterceptorMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public TrafficInterceptorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var originalBodyStream = context.Response.Body;
            //Create dummy MemmoryStream because the response stream can only be read once
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var response = await FormatResponse(context.Response);
            await responseBody.CopyToAsync(originalBodyStream);

            sw.Stop();

            var request = context.Request;
        }

        #endregion

        #region Private Methods

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            if (!IsResponseStatusSuccessful(response.StatusCode))
            {
                return $"{response.StatusCode} {text}";
            }
            else
            {
                int recordCount = 0;
                if (!string.IsNullOrEmpty(text))
                {
                    var token = JToken.Parse(text);
                    if (token is JArray jArray)
                    {
                        recordCount = jArray.Count;
                    }
                    else if (token is JObject jObject)
                    {
                        recordCount = jObject.Count == 0 ? 0 : 1;
                    }
                }

                return $"#{recordCount}";
            }
        }

        private bool IsResponseStatusSuccessful(int statusCode)
        {
            return statusCode >= (int)HttpStatusCode.OK && statusCode <= 299;
        }

        #endregion
    }
}
