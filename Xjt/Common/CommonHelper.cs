using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Xjt.Areas.Manager.Models;

namespace Xjt.Common
{
    public class CommonHelper
    {
        public static Dictionary<string, string> SignDic = new Dictionary<string, string>();

        public static JsonResult Result(object obj)
        {
            return Result(ResultType.Success, obj);
        }

        public static JsonResult ExceptionResult(object obj)
        {
            return Result(ResultType.Exception, obj);
        }

        public static JsonResult Result(ResultType type, object obj)
        {
            var jsonResult = new JsonResult
            {
                ContentType = "application/json",
                Data = new { code = (int)type, data = JsonConvert.SerializeObject(obj) }
            };

            return jsonResult;
        }

        public static T GetJsonModel<T>(string fileName)
        {
            var value = default(T);
            var path = HostingEnvironment.MapPath($"/Data/{fileName}.config");
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                content = content.Replace("\\r\\n", string.Empty);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }

            return value;
        }
    }
}