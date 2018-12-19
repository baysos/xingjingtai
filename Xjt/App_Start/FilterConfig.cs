using System.Web;
using System.Web.Mvc;
using Xjt.Areas.Manager.Controllers;
using Xjt.Common;

namespace Xjt
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyActionFilter());
        }
    }
}
