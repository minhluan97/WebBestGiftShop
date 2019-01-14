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
    public class QuanLiDanhMucController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: Admin/QuanLiDanhMuc
        public ActionResult Index()
        {
            
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            var res = data.DANHMUCs.ToList();
            return View(res);
        }

        public ActionResult SanPhamTheoDanhMuc(int id,int ? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 5;
                int pageNum = (page ?? 1);
                var res = data.SANPHAMs.Where(a => a.MA_DM == id).ToList();
                return View(res.ToPagedList(pageNum, pageSize));
                //return View(res);
            }
        }
    }
}