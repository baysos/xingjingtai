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

                user.LastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CommonHelper.SaveJsonModel(userList, "User");
                var sign = Guid.NewGuid().ToStringEx();
                CommonHelper.SignDic[name] = sign;

                return CommonHelper.Result(sign);
            }

            return CommonHelper.ExceptionResult("用户名不存在");
        }

        [HttpPost]
        [IdentityVerificationAjax]
        public ActionResult ChangePass(string name, string oldPass, string newPass)
        {
            var userList = CommonHelper.GetJsonModel<List<User>>("User");
            if (userList != null && userList.Count > 0)
            {
                var user = userList.Find(a => a.Name == name);
                if (user == null)
                {
                    return CommonHelper.ExceptionResult("用户名不存在");
                }

                var p = SecurityHelper.CreateMd5Str(oldPass + ConstValue.PassSalt);
                if (user.Pass != p)
                {
                    return CommonHelper.ExceptionResult("原密码验证错误");
                }

                p = SecurityHelper.CreateMd5Str(newPass + ConstValue.PassSalt);
                user.Pass = p;
                CommonHelper.SaveJsonModel(userList, "User");

                return CommonHelper.Result("修改成功！");
            }

            return CommonHelper.ExceptionResult("用户数据有误！");
        }

        #endregion

        #region 产品相关

        [IdentityVerification]
        public ActionResult Product()
        {
            var list = CommonHelper.GetJsonModel<List<Product>>("Product");
            ViewBag.List = list?.OrderBy(a => a.Id).ToList();
            return View();
        }

        [IdentityVerification]
        public ActionResult AddProduct()
        {
            return View();
        }

        /// <summary>
        /// 添加 修改 产品信息
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [IdentityVerificationAjax]
        public ActionResult UpdateProductData(Product product)
        {
            if (product == null)
            {
                return CommonHelper.ExceptionResult("参数有误！");
            }

            try
            {
                Data.Data.ClearDataByType(DataTypeEnum.Product);
                var list = CommonHelper.GetJsonModel<List<Product>>("Product");

                //添加
                if (product.Id == 0)
                {
                    var id = 0;
                    if (list != null && list.Count > 0)
                    {
                        list = list.OrderByDescending(a => a.Id).ToList();
                        id = list[0].Id + 1;
                        product.Id = id;
                    }
                    else
                    {
                        list = new List<Product>();
                        product.Id = 1;
                    }

                    product.Order = product.Id;
                    list.Add(product);
                }
                else //修改
                {
                    var pdt = list.Find(a => a.Id == product.Id);
                    if (pdt != null)
                    {
                        list.RemoveAll(a => a.Id == product.Id);
                        product.Order = pdt.Order;
                        list.Add(product);
                    }
                }
                CommonHelper.SaveJsonModel(list, "Product");

                return CommonHelper.Result("添加成功！");
            }
            catch (Exception e)
            {
                return CommonHelper.ExceptionResult("添加失败！e:" + e.Message + "||" + e.InnerException?.Message);
            }
            
        }

        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [IdentityVerificationAjax]
        public ActionResult DelProductData(int id)
        {
            try
            {
                Data.Data.ClearDataByType(DataTypeEnum.Product);
                var list = CommonHelper.GetJsonModel<List<Product>>("Product");
                var removeModel = list.Find(a => a.Id == id);
                var img = removeModel != null ? removeModel.Img : string.Empty;
                list.RemoveAll(a => a.Id == id);

                CommonHelper.SaveJsonModel(list, "Product");

                if (!string.IsNullOrWhiteSpace(img))
                {
                    //删除文件
                    var imgPath = HostingEnvironment.MapPath(img);
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                }

                return CommonHelper.Result("删除成功！");
            }
            catch (Exception e)
            {
                return CommonHelper.ExceptionResult("删除失败！e:" + e.Message + "||" + e.InnerException?.Message);
            }
            
        }

        /// <summary>
        /// 产品重排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [IdentityVerificationAjax]
        public ActionResult OrderProduct(int id, int type)
        {
            try
            {
                Data.Data.ClearDataByType(DataTypeEnum.Product);
                var list = CommonHelper.GetJsonModel<List<Product>>("Product");
                var isUpdate = false;
                if (list != null && list.Count > 0)
                {
                    var model = list.Find(a => a.Id == id);
                    if (model != null)
                    {
                        Product md = null;
                        //上移
                        if (type == 1)
                        {
                            var listTemp = list.FindAll(a => a.Order < model.Order).OrderByDescending(a => a.Order)
                                .ToList();

                            md = listTemp.Count > 0 ? listTemp[0] : null;
                        }
                        else //下移
                        {
                            var listTemp = list.FindAll(a => a.Order > model.Order).OrderBy(a => a.Order).ToList();

                            md = listTemp.Count > 0 ? listTemp[0] : null;
                        }

                        if (md != null)
                        {
                            md.Order = md.Order + model.Order;
                            model.Order = md.Order - model.Order;
                            list.Find(a => a.Id == md.Id).Order = md.Order - model.Order;
                            isUpdate = true;
                        }

                        CommonHelper.SaveJsonModel(list, "Product");
                    }
                }

                if (isUpdate)
                {
                    return CommonHelper.Result("操作成功");
                }
                else
                {
                    return CommonHelper.Result("未操作");
                }
            }
            catch (Exception e)
            {
                return CommonHelper.ExceptionResult("删除失败！e:" + e.Message + "||" + e.InnerException?.Message);
            }
            
        }

        [HttpPost]
        [IdentityVerificationAjax]
        public ActionResult ProductUploadFile(HttpPostedFileBase file)
        {
            try
            {
                var pt = "/Image/UpLoad";
                var maxSize = 1 * 1024 * 1000;
                if (file.ContentLength > maxSize)
                {
                    return CommonHelper.ExceptionResult(new { msg = "上传失败" });
                }

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

        [IdentityVerification]
        public ActionResult Index()
        {
            var banner = CommonHelper.GetJsonModel<Banner>("Banner");
            if (banner != null)
            {
                ViewBag.Banner = banner;
            }
            return View();
        }

        [HttpPost]
        [IdentityVerificationAjax]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                Data.Data.ClearDataByType(DataTypeEnum.Banner);
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

        #region 其他设置

        [IdentityVerification]
        public ActionResult Other()
        {
            return View();
        }
        
        [HttpPost]
        [IdentityVerificationAjax]
        public ActionResult UpdateOtherData(Other model)
        {
            if (model == null)
            {
                return CommonHelper.ExceptionResult("参数有误");
            }
            Data.Data.ClearDataByType(DataTypeEnum.Other);
            CommonHelper.SaveJsonModel(model, "Other");
            return CommonHelper.Result("更新成功！");
        }

        #endregion

    }
}