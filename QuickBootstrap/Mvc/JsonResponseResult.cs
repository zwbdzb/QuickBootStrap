using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QuickBootstrap.Mvc
{
    public class JsonResponseResult : ActionResult
    {
#if DEBUG
        public static bool AllowOrigin
        {
            get { return WebConfigurationManager.AppSettings["AllowOrigin"] == "true"; }
        }
#endif

        public int StatusCode { get; set; }

        public string JsonContent { get; set; }

        public JsonResponseResult(int statusCode, string jsonContent)
        {
            StatusCode = statusCode;
            JsonContent = jsonContent;
        }

        public override void ExecuteResult(ControllerContext context)
        {
#if DEBUG
            if (AllowOrigin)
            {
                var origin = HttpContext.Current.Request.Headers.Get("Origin");
                if (!string.IsNullOrEmpty(origin))
                {
                    context.RequestContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                }
                context.RequestContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS, DELETE, PUT, PATCH");
                context.RequestContext.HttpContext.Response.Headers.Add("Access-Control-Max-Age", "3600");
                context.RequestContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "content-type, x-requested-with, Cookie");
                context.RequestContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
#endif
            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;
            context.HttpContext.Response.StatusCode = StatusCode;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(JsonContent);
        }
    }
}