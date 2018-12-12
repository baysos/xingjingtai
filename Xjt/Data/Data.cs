using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xjt.Data
{
    public class Data
    {

    }

    public class User
    {
        public string Name { get; set; }

        public string Pass { get; set; }

        public string LastLoginTime { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public string Img { get; set; }
    }

    public class Banner
    {
        public string Img { get; set; }

        public string Title { get; set; }
    }
}