using Newtonsoft.Json;

namespace QuickBootstrap.Mvc
{
    public class ResponseError
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("msg")]
        public string Msg { get; set; }
    }
}