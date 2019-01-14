using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopQuaTang.Areas.Admin.Models;
using System.Web.Security;

namespace ShopQuaTang.Areas.Admin.Controllers
{
    public class AdminShopController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: Admin/Admin

        public ActionResult ThongTinNVPartial()
        {
            NHANVIEN nv = (NHANVIEN)Session["TaiKhoanNV"];
            if (nv == null)
            {
                return PartialView();
            }
            else
            {
                var query = data.NHANVIENs.SingleOrDefault(a => a.MA_NV == (nv.MA_NV));
                return PartialView(query);
            }
        }

        public ActionResult Index()
        {
            var CountSP = data.SANPHAMs.Where(a => a.STATUS_SP == true).Count();
            ViewBag.SoLuongSP = (int)CountSP;
            var CountKH = data.KHACHHANGs.Count();
            ViewBag.SoLuongKH = (int)CountKH;
            var CountHD = data.HOADONs.Where(a => a.NGAYLAP_HD.Value.Month == (int)DateTime.Now.Month).Count();
            ViewBag.SoLuongHD = (int)CountHD;
            var SumDoanhThu = data.HOADONs.Where(a => a.NGAYLAP_HD.Value.Month == (int)DateTime.Now.Month).Sum(a=>a.TONGTIEN_HD);
            //ViewBag.TongDoanhThu = (float)SumDoanhThu;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var namelogin = collection["USERNAME_NV"];
            var passlogin = collection["PASSWORD_NV"];
            NHANVIEN nv = data.NHANVIENs.SingleOrDefault(n => n.USERNAME_NV == namelogin && n.PASSWORD_NV == ShopQuaTang.Models.EncryptMD5.MD5Hash(passlogin) &&n.STT_NV==true);
            if (nv != null)
            {
                Session["TaiKhoanNV"] = nv;
                return RedirectToAction("Index", "AdminShop");
            }
            else
            {
                ViewBag.ThongBao = "Kiểm Tra Lại Username và Password";
            }
            return RedirectToAction("Login", "AdminShop");
        }
        public ActionResult Logout()
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            
            return RedirectToAction("Login", "AdminShop");
        }

        
    }
}