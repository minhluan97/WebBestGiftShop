using PagedList;
using ShopQuaTang.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopQuaTang.Areas.Admin.Controllers
{
    public class QuanLiTinTucController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: Admin/QuanLiTinTuc
        public ActionResult Index(int? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 8;
                int pageNum = (page ?? 1);
                var res = from tt in data.TINTUCs.OrderBy(a => a.MA_TIN).Where(a => a.STATUS_TIN == true) select tt;

                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult ThemTinTuc()
        {

            return View();
        }
    }
}