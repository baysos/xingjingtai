using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Xjt.Common;
using Xjt.Data;

namespace Xjt.Areas.Manager.Controllers
{
    public class DataController : BaseController
    {
        // GET: Manager/Data
        public ActionResult Index()
        {
            var banner = CommonHelper.GetJsonModel<Banner>("Banner");
            if (banner != null)
            {
                ViewBag.Banner = banner;
            }
            return View();
        }

        #region 用户相关

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

        #endregion

        #region 产品相关

        public ActionResult Product()
        {
            var list = CommonHelper.GetJsonModel<List<Product>>("Product");
            ViewBag.List = list?.OrderBy(a => a.Id).ToList();
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductUploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (!CheckParamSign())
                {
                    return CommonHelper.ExceptionResult(new { msg = "上传失败,无权限！" });
                }

                var maxSize = 1 * 1024 * 1000;
                if (file.ContentLength > maxSize)
                {
                    return CommonHelper.ExceptionResult(new { msg = "上传失败" });
                }

                var pt = "/Image/UpLoad";
                var fileName = file.FileName;
                var filePath = Server.MapPath(pt);
                var path = Path.Combine(filePath, fileName);
                file.SaveAs(path);

                return CommonHelper.Result(new { msg = "上传成功", path = path });
            }
            catch (Exception e)
            {
                return CommonHelper.ExceptionResult(new { msg = "上传失败" });
            }

        }

        #endregion

        #region Banner相关

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (!CheckParamSign())
                {
                    return CommonHelper.ExceptionResult(new { msg = "上传失败,无权限！" });
                }

                var maxSizeSetting = ConfigurationManager.AppSettings["BannerImgMaxLength"];
                var maxSize = 0;
                maxSize = maxSizeSetting == null ? 5120000 : TryConvertUtil.ToInt(maxSizeSetting.ToStringEx());
                if (file.ContentLength > maxSize)
                {
                    return CommonHelper.ExceptionResult(new { msg = "上传失败" });
                }

                var banner = CommonHelper.GetJsonModel<Banner>("Banner");
                if (banner != null)
                {
                    var bannerPath = HostingEnvironment.MapPath(banner.Img);
                    if (System.IO.File.Exists(bannerPath))
                    {
                        System.IO.File.Delete(bannerPath);
                    }

                }

                var pt = "/Image/UpLoad/Banner";
                var fileName = file.FileName;
                var filePath = Server.MapPath(pt);
                var path = Path.Combine(filePath, fileName);
                file.SaveAs(path);

                banner = new Banner
                {
                    Img = pt + "/" + fileName,
                    Title = fileName.Substring(0, fileName.LastIndexOf('.'))
                };
                CommonHelper.SaveJsonModel(banner, "Banner");

                return CommonHelper.Result(new { msg = "上传成功", path = path });
            }
            catch (Exception e)
            {
                return CommonHelper.ExceptionResult(new { msg = "上传失败" });
            }

        }

        #endregion

    }
}