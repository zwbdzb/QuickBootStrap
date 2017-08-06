using System.Linq;
using System.Web;
using QuickBootstrap.Services.Util;

namespace QuickBootstrap.Extendsions
{
    public static class ExtensionMethod
    {
        public static string GetQueryString(this  object obj, string[] props)
        {
            if (props != null)
            {
                var properties = from p in obj.GetType().GetProperties()
                                 where props.Contains(p.Name) && p.GetValue(obj, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
                return string.Join("&", properties.ToArray());
            }
            else
            {
                var properties = from p in obj.GetType().GetProperties()
                                 where p.GetValue(obj, null) != null
                                 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
                return string.Join("&", properties.ToArray());
            }
        }
    }

    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return new PagedList<T>(source, page, pageSize);
        }
    }

}