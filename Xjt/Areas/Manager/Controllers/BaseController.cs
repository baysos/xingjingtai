using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xjt.Common;

namespace Xjt.Areas.Manager.Controllers
{
    public class BaseController: Controller
    {
        public bool CheckParamSign()
        {
            var sign = Request.Params["s"];
            var name = Request.Params["n"];
            if (CommonHelper.SignDic.ContainsKey(name))
            {
                if (CommonHelper.SignDic[name] != sign)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

            return true;
        }
    }
}