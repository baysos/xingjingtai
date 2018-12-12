using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Xjt.Models;

namespace Xjt.Common
{
    public class CustomActionFilterAttribute: ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var ivAttrs = actionContext.ActionDescriptor.GetCustomAttributes<IdentityVerification>();
            if (ivAttrs != null && ivAttrs.Count > 0)
            {
                actionContext.ActionArguments.TryGetValue("pModel", out var paramObj);
                if (paramObj is ApiParamModel<object> paramModel)
                {
                    CommonHelper.SignDic.TryGetValue(paramModel.account, out string signVal);
                    if (string.IsNullOrWhiteSpace(paramModel.account) || string.IsNullOrWhiteSpace(signVal) ||
                        paramModel.sign != signVal)
                    {
                        var jsonResult = JsonConvert.SerializeObject(new {code = 500, msg = "参数异常"});
                        actionContext.Response = new HttpResponseMessage
                        {
                            Content = new StringContent(jsonResult, Encoding.GetEncoding("UTF-8"), "application/json")
                        };
                    }
                }
            }
        }
    }
}