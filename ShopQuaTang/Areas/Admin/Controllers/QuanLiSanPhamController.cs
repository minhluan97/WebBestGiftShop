using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopQuaTang.Areas.Admin.Models;

using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI.WebControls;

namespace ShopQuaTang.Areas.Admin.Controllers
{
    public class QuanLiSanPhamController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();

        // GET: Admin/QuanLiSanPham
        public ActionResult ChonSanPham(int? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 20;
                int pageNum = (page ?? 1);
                var res = data.SANPHAMs.Where(a => a.STATUS_SP == true).OrderBy(a=>a.SL_TON).ToList();
                return View(res.ToPagedList(pageNum, pageSize));
            }

        }
        public ActionResult Index(int ? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 8;
                int pageNum = (page ?? 1);
                var res = from sp in data.SANPHAMs.OrderBy(a=>a.MA_SP).Where(a=>a.STATUS_SP==true) select sp;
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        [HttpGet]
        public ActionResult ThemSanPham()
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            ViewBag.MA_DM = new SelectList(data.DANHMUCs.ToList().OrderBy(a => a.MA_DM),"MA_DM","TEN_DM");// Ten view bag trung mã danh mục trong sql
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            ViewBag.MA_DM = new SelectList(data.DANHMUCs.ToList().OrderBy(a => a.MA_DM),"MA_DM","TEN_DM");//

            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Please choose avatar";
                return View();
            }
            else
            {

                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Upload"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Existed";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                sp.AVA_SP = fileName;
                data.SANPHAMs.InsertOnSubmit(sp);
                data.SubmitChanges();
            }

            return RedirectToAction("Index", "QuanLiSanPham");
        }
        [HttpGet]
        public ActionResult SuaSanPham(int id)
        {
            
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(a => a.MA_SP == id);
            if (sp.STATUS_SP == false)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Index");
            }
            ViewBag.MA_DM = new SelectList(data.DANHMUCs.ToList().OrderBy(a => a.MA_DM), "MA_DM", "TEN_DM",sp.MA_DM);// Ten view bag trung mã danh mục trong sql
            return View(sp);
        } 
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham(int id,HttpPostedFileBase fileUpload)
        {
            ViewBag.MA_DM = new SelectList(data.DANHMUCs.ToList().OrderBy(a => a.MA_DM), "MA_DM", "TEN_DM");

            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Please choose avatar";
                return View();
            }
            else
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Upload"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Existed";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                SANPHAM sp = data.SANPHAMs.SingleOrDefault(a => a.MA_SP == id);
                sp.AVA_SP = fileName;
                sp.STATUS_SP = true;
                sp.NGAYCAPNHAT_SP = DateTime.Now;
                UpdateModel(sp);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult ChiTietSanPham(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            var res = data.SANPHAMs.ToList().Where(a => a.MA_SP == id);
            return View(res.Single());
        }
        public ActionResult LoadAnh(int id)
        {
            var res = from ha in data.HINHANH_SPs where ha.MA_SP == id where ha.STT_HA==true select ha;
            SANPHAM sp = data.SANPHAMs.First(m => m.MA_SP == id);
            ViewBag.MaDM = sp.MA_DM;
            return PartialView(res);
        }
        public ActionResult XoaSanPham(int id)
        {
            
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(m => m.MA_SP == id);
            sp.STATUS_SP = false;
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult XoaAnh(int id,string name)
        {

            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            HINHANH_SP ha = data.HINHANH_SPs.Where(a=>a.MA_SP==id).Where(a=>a.ANH_SP==name).Single();
            ha.STT_HA = false;
            UpdateModel(ha);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult LichSuXoa(int ? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 8;
                int pageNum = (page ?? 1);
                var res = data.SANPHAMs.Where(a => a.STATUS_SP == false).ToList();
                return View(res.ToPagedList(pageNum, pageSize));
            }
            
        }
        public ActionResult LayLaiSanPham(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(m => m.MA_SP == id);
            sp.STATUS_SP = true;
            sp.GIA_SP = sp.GIA_SP + 10000;
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UploadAnh(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            var res = data.SANPHAMs.ToList().Where(a => a.MA_SP == id);
            return View(res.Single());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadAnh(int id, HINHANH_SP ha,HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Please choose avatar";
                return View();
            }
            else
            {
                
                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Upload"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Existed";
                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                ha.MA_SP = id;
                ha.ANH_SP = fileName;
                ha.STT_HA = true;
                data.HINHANH_SPs.InsertOnSubmit(ha);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}