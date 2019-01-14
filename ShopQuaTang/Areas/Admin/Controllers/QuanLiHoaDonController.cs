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
    public class QuanLiHoaDonController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: Admin/QuanLiHoaDon
        //duyet hoa don

        public ActionResult Index(int ? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 6;
                int pageNum = (page ?? 1);
                var res = from hd in data.HOADONs.OrderByDescending(a => a.NGAYLAP_HD).Where(a => a.STATUS_HD == false).Where(a => a.STATUS_GH == false).Where(a=>a.MA_NV==null).ToList() select hd;
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult XuLiHoaDon(int? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 6;
                int pageNum = (page ?? 1);
                var res = from hd in data.HOADONs.OrderByDescending(a => a.NGAYLAP_HD).Where(a => a.STATUS_HD == false).Where(a => a.MA_NV == null).ToList() select hd;
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult XuLiGiaoHang(int? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 6;
                int pageNum = (page ?? 1);
                var res = from hd in data.HOADONs.OrderByDescending(a => a.NGAYLAP_HD).Where(a => a.STATUS_GH == false).ToList() select hd;
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult XemHoaDon(int? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 6;
                int pageNum = (page ?? 1);
                var res = from hd in data.HOADONs.OrderByDescending(a=>a.MA_HD).Where(a => a.STATUS_HD == true).Where(a => a.STATUS_GH == true).Where(a => a.MA_NV != null).ToList() select hd;
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult ChiTietHoaDon(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                var res = data.CT_HOADONs.ToList().Where(a=>a.MA_HD==id);
                return View(res);
            }
        }
        public ActionResult XacNhanHoaDon(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                NHANVIEN nv = (NHANVIEN)Session["TaiKhoanNV"];
                HOADON hd = data.HOADONs.SingleOrDefault(a => a.MA_HD == id);
                hd.STATUS_HD = true;
                hd.MA_NV = nv.MA_NV;
                UpdateModel(hd);
                data.SubmitChanges();
            }
            return RedirectToAction("XuLiHoaDon");
        }
        public ActionResult XacNhanGiaoHang(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                NHANVIEN nv = (NHANVIEN)Session["TaiKhoanNV"];
                HOADON hd = data.HOADONs.SingleOrDefault(a => a.MA_HD == id);
                hd.STATUS_GH = true;
                UpdateModel(hd);
                data.SubmitChanges();
            }
            return RedirectToAction("XuLiGiaoHang");
        }
        private double ThongKeTheoThang(int month, int year)
        {
            double b;
            var res = data.HOADONs.Where(a => a.NGAYLAP_HD.Value.Month == month).Where(a => a.NGAYLAP_HD.Value.Year == year).Sum(a => a.TONGTIEN_HD);
            b = Convert.ToDouble(res);
            return b;
        }
        public ActionResult ThongKeHoaDon()
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                ViewBag.T1 = ThongKeTheoThang(1, 2018);
                ViewBag.T2 = ThongKeTheoThang(2, 2018);
                ViewBag.T3 = ThongKeTheoThang(3, 2018);
                ViewBag.T4 = ThongKeTheoThang(4, 2018);
                ViewBag.T5 = ThongKeTheoThang(5, 2018);
                ViewBag.T6 = ThongKeTheoThang(6, 2018);
                ViewBag.T7 = ThongKeTheoThang(7, 2018);
                ViewBag.T8 = ThongKeTheoThang(8, 2018);
                ViewBag.T9 = ThongKeTheoThang(9, 2018);
                ViewBag.T10 = ThongKeTheoThang(10, 2018);
                ViewBag.T11 = ThongKeTheoThang(11, 2018);
                ViewBag.T12 = ThongKeTheoThang(12, 2018);

                return View();
            }
        }

    }
}