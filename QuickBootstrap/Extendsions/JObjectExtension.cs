using Newtonsoft.Json.Linq;

namespace QuickBootstrap.Extendsions
{
    public static class JObjectExtension
    {
        public static string GetString(this JObject obj, string field)
        {
            if (obj == null)
            {
                return null;
            }
            var value = obj[field];
            return value == null ? null : value.ToString();
        }
    }
}