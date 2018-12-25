using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xjt.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult IndustryCase()
        {
            return View();
        }

        public ActionResult JoinUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult ProductCenter()
        {
            return View();
        }

        public ActionResult ProductDetail()
        {
            return View();
        }
    }
}
