using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xjt.Areas.Manager.Models
{
    public enum ResultType
    {
        Success = 200,
        Authorization = 302,
        Exception = 500
    }
}