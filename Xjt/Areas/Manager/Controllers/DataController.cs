using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xjt.Common;

namespace Xjt.Areas.Manager.Controllers
{
    public class DataController : Controller
    {
        // GET: Manager/Data
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var p = "xjt123456";
            ViewBag.pass = SecurityHelper.CreateMd5Str(p + ConstValue.PassSalt);
            return View();
        }
    }
}