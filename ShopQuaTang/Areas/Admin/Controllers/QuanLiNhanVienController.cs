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
    public class QuanLiNhanVienController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: Admin/QuanLiNhanVien
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
                var res = data.NHANVIENs.ToList();
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        [HttpGet]
        public ActionResult ThemNhanVien()
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNhanVien(FormCollection collection)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                string hotennv = collection["HOTEN_NV"];
                string cmt = collection["CMMD_NV"];
                string usernamenv = collection["USERNAME_NV"];
                string newpass = collection["newpass"];
                string confirmpass = collection["confirmpass"];
                NHANVIEN nv = new NHANVIEN();
                if (String.Compare(newpass, confirmpass, true) == 0)
                {
                    nv.HOTEN_NV = hotennv;
                    nv.CMMD_NV = cmt;
                    nv.USERNAME_NV = usernamenv;
                    nv.PASSWORD_NV = BamMD5.MD5Hash(newpass);
                    nv.STT_NV = true;
                    data.NHANVIENs.InsertOnSubmit(nv);
                    data.SubmitChanges();
                }
                else
                {
                    ViewData["Loi"] = "Không khớp password";
                }

                return RedirectToAction("Index");
            }
        }
        public ActionResult XoaNhanVien(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                NHANVIEN res = data.NHANVIENs.SingleOrDefault(a=>a.MA_NV==id);
                res.STT_NV = false;
                UpdateModel(res);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult RestoreNhanVien(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                NHANVIEN res = data.NHANVIENs.SingleOrDefault(a => a.MA_NV == id);
                res.STT_NV = true;
                UpdateModel(res);
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult SuaNhanVien(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                NHANVIEN res = data.NHANVIENs.SingleOrDefault(a => a.MA_NV == id);
                return View(res);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNhanVien(int id,FormCollection collection)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                string hotennv = collection["HOTEN_NV"];
                string newpass = collection["newpass"];
                string confirmpass = collection["confirmpass"];
                if (String.Compare(newpass, confirmpass, true) == 0)
                {
                    NHANVIEN nv = data.NHANVIENs.Single(a => a.MA_NV == id);
                    nv.HOTEN_NV = hotennv;
                    nv.PASSWORD_NV = BamMD5.MD5Hash(newpass);
                    nv.STT_NV = true;
                    UpdateModel(nv);
                    data.SubmitChanges();
                }
                else
                {
                    ViewData["Loi"] = "Không khớp password";
                }
                ViewData["DoiThanhCong"] = "Đổi Thành Công!";
                return RedirectToAction("Index");
            }
        }
    }
}