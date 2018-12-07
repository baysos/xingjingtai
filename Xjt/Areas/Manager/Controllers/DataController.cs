using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Xjt.Areas.Manager.Models;
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

        public ActionResult Login(string name, string pass)
        {
            if (Request.IsAjaxRequest())
            {
                Response.Redirect("/Manager/data/index");
            }
            return View();
        }

        private JsonResult Result(ResultType type, object obj)
        {
            var jsonResult = new JsonResult();
            jsonResult.ContentType = "application/json";
            jsonResult.Data = new { code = (int)type, data = JsonConvert.SerializeObject(obj) };

            return jsonResult;
        }
    }
}