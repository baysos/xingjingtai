using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xjt.Models
{
    public class ApiParamModel<TData>
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        public string account { get; set; }

        /// <summary>
        /// 传输具体数据
        /// </summary>
        public TData data { get; set; }
    }
}