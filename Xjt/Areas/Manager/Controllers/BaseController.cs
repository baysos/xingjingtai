using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xjt.Areas.Manager.Models;
using Xjt.Common;

namespace Xjt.Areas.Manager.Controllers
{
    public class BaseController: Controller
    {

    }

    public class MyActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            object[] att = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IdentityVerification), false);
            if (att.Length > 0)
            {
                var sign = filterContext.HttpContext.Request.Params["s"].ToStringEx();
                var name = filterContext.HttpContext.Request.Params["n"].ToStringEx();
                if (CommonHelper.SignDic.ContainsKey(name))
                {
                    if (CommonHelper.SignDic[name] != sign)
                    {
                        
                        filterContext.Result = new RedirectResult("/manager/data/login");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/manager/data/login");
                }
            }

            att = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IdentityVerificationAjax), false);
            if (att.Length > 0)
            {
                var sign = filterContext.HttpContext.Request.Params["s"].ToStringEx();
                var name = filterContext.HttpContext.Request.Params["n"].ToStringEx();
                if (CommonHelper.SignDic.ContainsKey(name))
                {
                    if (CommonHelper.SignDic[name] != sign)
                    {

                        filterContext.Result = CommonHelper.Result(ResultType.Authorization, "/manager/data/login");
                    }
                }
                else
                {
                    filterContext.Result = CommonHelper.Result(ResultType.Authorization, "/manager/data/login");
                }
            }
        }
    }
}