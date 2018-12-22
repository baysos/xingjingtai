using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xjt.Common;

namespace Xjt.Data
{
    public class Data
    {
        private static Banner _banner;
        public static Banner Banner => _banner ?? (_banner = CommonHelper.GetJsonModel<Banner>("Banner"));

        private static List<Product> _productList;
        public static List<Product> ProductList => _productList ?? (_productList = CommonHelper.GetJsonModel<List<Product>>("Product"));

        private static Other _other;
        public static Other Other => _other ?? (_other = CommonHelper.GetJsonModel<Other>("Other"));

        public static void ClearDataByType(DataTypeEnum dt)
        {
            switch (dt)
            {
                case DataTypeEnum.Banner:
                    _banner = null;
                    break;
                case DataTypeEnum.Product:
                    _productList = null;
                    break;
                case DataTypeEnum.Other:
                    _other = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dt), dt, null);
            }
        }
    }

    public class User
    {
        public string Name { get; set; }

        public string Pass { get; set; }

        public string LastLoginTime { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 封装
        /// </summary>
        public string Encapsulation { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// 输入电压
        /// </summary>
        public string InputVoltage { get; set; }

        /// <summary>
        /// 输出电压
        /// </summary>
        public string OutputVoltage { get; set; }

        /// <summary>
        /// 输出电流
        /// </summary>
        public string OutputElectric { get; set; }

        /// <summary>
        /// 库存信息
        /// </summary>
        public string Stock { get; set; }

        public string Content { get; set; }

        public string Img { get; set; }
    }

    public class Banner
    {
        public string Img { get; set; }

        public string Title { get; set; }
    }

    public class Other
    {
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
    }

    public enum DataTypeEnum
    {
        User = 1,
        Banner = 2,
        Product = 3,
        Other = 4
    }
}