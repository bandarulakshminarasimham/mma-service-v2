using MeetingService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace MeetingService.App_Start
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            return BuildApiResponse(request, response);
        }
        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            string errorMessage = null;
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;
                if (error != null)
                {
                    content = null;
                    if (error.ModelState != null)
                    {
                        errorMessage = String.Join(",", ((String[])error.ModelState.Values.ToArray().FirstOrDefault()));

                    }
                    else 
                    {
                        errorMessage = error.Message;
                    }
#if DEBUG
                    errorMessage = string.Concat(errorMessage, error.ExceptionMessage, error.StackTrace);
#endif
                    /// tracking all model state errors
                    GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
                    var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
                    trace.Info(request, errorMessage, "JSON", request.Headers);
                }
            }
            var newResponse = request.CreateResponse(response.StatusCode, new ApiResponse(response.StatusCode, content, errorMessage));
            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }
            return newResponse;
        }
    }
}