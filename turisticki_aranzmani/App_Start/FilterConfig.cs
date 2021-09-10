using System.Web;
using System.Web.Mvc;

namespace turisticki_aranzmani
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
