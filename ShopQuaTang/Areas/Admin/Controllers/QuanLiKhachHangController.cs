using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopQuaTang.Areas.Admin.Models;
using PagedList;
using PagedList.Mvc;
namespace ShopQuaTang.Areas.Admin.Controllers
{
    public class QuanLiKhachHangController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: Admin/QuanLiKhachHang
        public ActionResult Index(int ? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 10;
                int pageNum = (page ?? 1);
                var res = data.KHACHHANGs.OrderBy(a => a.MA_KH).ToList();
                return View(res.ToPagedList(pageNum, pageSize));
            }
            
        }
    }
}