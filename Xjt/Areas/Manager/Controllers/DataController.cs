using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using Xjt.Areas.Manager.Models;
using Xjt.Common;
using Xjt.Data;

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
            return View();
        }

        [HttpPost]
        public ActionResult LoginCheck(string name, string pass)
        {
            var userList = CommonHelper.GetJsonModel<List<User>>("User");
            if (userList != null && userList.Count > 0)
            {
                var user = userList.Find(a => a.Name == name);
                if (user == null)
                {
                    return CommonHelper.ExceptionResult("用户名不存在");
                }

                var p = SecurityHelper.CreateMd5Str(pass + ConstValue.PassSalt);
                if (user.Pass != p)
                {
                    return CommonHelper.ExceptionResult("用户名或密码错误");
                }

                var sign = Guid.NewGuid().ToStringEx();
                CommonHelper.SignDic[name] = sign;

                return CommonHelper.Result(sign);
            }

            return CommonHelper.ExceptionResult("用户名不存在");
        }
    }
}