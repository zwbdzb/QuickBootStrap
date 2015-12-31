using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace QuickBootstrap.Mvc
{
    public class JsonApiResult : IHttpActionResult
    {
#if DEBUG
        public static bool AllowOrigin
        {
            get { return WebConfigurationManager.AppSettings["AllowOrigin"] == "true"; }
        }
#endif
        public HttpStatusCode StatusCode { get; private set; }

        public string Content { get; private set; }

        public JsonApiResult(HttpStatusCode code, string content)
        {
            StatusCode = code;
            Content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(StatusCode);
#if DEBUG
            if (AllowOrigin)
            {
                var origin = HttpContext.Current.Request.Headers.Get("Origin");
                if (!string.IsNullOrEmpty(origin))
                {
                    response.Headers.Add("Access-Control-Allow-Origin", origin);
                }
                response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS, DELETE, PUT, PATCH");
                response.Headers.Add("Access-Control-Max-Age", "3600");
                response.Headers.Add("Access-Control-Allow-Headers", "content-type, x-requested-with, Cookie");
                response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
#endif
            response.Content = new StringContent(Content, Encoding.UTF8, "application/json");
            return Task.FromResult(response);
        }
    }
}