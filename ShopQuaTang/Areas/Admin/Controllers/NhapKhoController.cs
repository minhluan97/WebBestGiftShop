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
    public class NhapKhoController : Controller
    {
        // GET: Admin/NhapKho
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        public ActionResult Index(int ?page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 6;
                int pageNum = (page ?? 1);
                var res = from pn in data.PHIEUNHAPs.OrderByDescending(a => a.NGAY_NHAP).ToList() select pn;
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult ChiTietNhapKho(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                var res = data.CT_PHIEUNHAPs.ToList().Where(a => a.MA_PN == id);
                return View(res);
            }
        }
        public List<NhapKho> ChonSanPham()
        {
            List<NhapKho> listNhapKho = Session["NhapKho"] as List<NhapKho>;
            if (listNhapKho == null)
            {
                listNhapKho = new List<NhapKho>();
                Session["NhapKho"] = listNhapKho;
            }
            return listNhapKho;
        }
        public ActionResult ThemNhapKho(int masp, string strURL)
        {
            List<NhapKho> listNhapKho = ChonSanPham();
            NhapKho nk = listNhapKho.Find(n => n.MaSP == masp);
            if (nk == null)
            {
                nk = new NhapKho(masp);
                listNhapKho.Add(nk);
                return Redirect(strURL);
            }
            else
            {
                nk.SoLuongSPNhap++;
                return Redirect(strURL);
            }
        }
        public ActionResult XoaNhapKho(int masp)
        {
            List<NhapKho> listNhapKho = ChonSanPham();
            NhapKho sp = listNhapKho.SingleOrDefault(n => n.MaSP == masp);
            if (sp != null)
            {
                listNhapKho.RemoveAll(a => a.MaSP == masp);
                return RedirectToAction("NhapKho");
            }
            if (listNhapKho.Count == 0)
            {
                return RedirectToAction("Index", "NhapKho");
            }
            return RedirectToAction("NhapKho");
        }
        public ActionResult CapNhatKho(int masp, FormCollection fc)
        {
            List<NhapKho> listNhapKho = ChonSanPham();
            NhapKho sp = listNhapKho.SingleOrDefault(n => n.MaSP == masp);
            if (sp != null)
            {
                sp.SoLuongSPNhap = int.Parse(fc["slnhap"].ToString());
            }
            return RedirectToAction("NhapKho");
        }
        private int TongSoLuong()
        {
            int tongsl = 0;
            List<NhapKho> listNhapKho = Session["NhapKho"] as List<NhapKho>;
            if (listNhapKho != null)
            {
                tongsl = listNhapKho.Sum(a => a.SoLuongSPNhap);
            }
            return tongsl;
        }
        private float TongTienPN()
        {
            float tongtien = 0;
            List<NhapKho> listNhapKho = Session["NhapKho"] as List<NhapKho>;
            if (listNhapKho != null)
            {
                tongtien = listNhapKho.Sum(a => a.ThanhTienPN);
            }
            return tongtien;
        }
        public ActionResult NhapKho()
        {
            List<NhapKho> listNhapKho = ChonSanPham();
            if (listNhapKho.Count == 0)
            {
                return RedirectToAction("Index", "NhapKho");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTienPN();
            return View(listNhapKho);
        }
        public ActionResult NhapKhoPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTienPN();
            return PartialView();
        }
        public ActionResult XoaTatCaNhapKho()
        {
            List<NhapKho> listNhapKho = ChonSanPham();
            listNhapKho.Clear();
            return RedirectToAction("ChonSanPham","QuanLiSanPham");
        }
        [HttpGet]
        public ActionResult GhiKho()
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            if (Session["NhapKho"] == null)
            {
                return RedirectToAction("ChonSanPham", "QuanLiSanPham");
            }
            List<NhapKho> listNhapKho = ChonSanPham();
            NHANVIEN nv = (NHANVIEN)Session["TaiKhoanNV"];
            ViewBag.MA_NCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(a => a.MA_NCC), "MA_NCC", "TEN_NCC");// Ten view bag trung mã danh mục trong sql
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTienPN();
            return View(listNhapKho);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GhiKho(FormCollection collection)
        {
            ViewBag.MA_NCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(a => a.MA_NCC), "MA_NCC", "TEN_NCC");// Ten view bag trung mã danh mục trong sql
            PHIEUNHAP pn = new PHIEUNHAP();
            NHANVIEN nv = (NHANVIEN)Session["TaiKhoanNV"];
            List<NhapKho> listNhapKho = ChonSanPham();
            
            //them phieu nhap kho
            pn.MA_NV = nv.MA_NV;
            pn.NGAY_NHAP = DateTime.Now;
            pn.TONGTIEN_PH = TongTienPN();
            pn.MA_NCC = int.Parse(collection["MA_NCC"]);
            data.PHIEUNHAPs.InsertOnSubmit(pn);
            data.SubmitChanges();

            //thêm CT phieu nhap
            foreach(var item in listNhapKho)
            {
                CT_PHIEUNHAP ctpn = new CT_PHIEUNHAP();
                SANPHAM sp = new SANPHAM();
                //update sl ton kho
                var capnhatSL = data.SANPHAMs.Single(a => a.MA_SP == item.MaSP);
                capnhatSL.SL_TON = capnhatSL.SL_TON + item.SoLuongSPNhap; 
                
                ctpn.MA_PN = pn.MA_PN;
                ctpn.MA_SP = item.MaSP;
                ctpn.SL_NHAP = item.SoLuongSPNhap;
                ctpn.GIA_NHAP = item.DonGiaSPNhap;
                ctpn.THANHTIEN = item.ThanhTienPN;
                data.CT_PHIEUNHAPs.InsertOnSubmit(ctpn);
            }
            data.SubmitChanges();
            Session["NhapKho"] = null;
            return RedirectToAction("Index","QuanLiSanPham");
        }

        public ActionResult DSNhaCungCap(int ? page)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                int pageSize = 6;
                int pageNum = (page ?? 1);
                var res = data.NHACUNGCAPs.OrderByDescending(a => a.MA_NCC).ToList();
                return View(res.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult ThemNhaCungCap()
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNhaCungCap(FormCollection collection)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                string a = collection["TEN_NCC"];
                string b = collection["DIACHI_NCC"];
                string c = collection["DT_NCC"];
              
                NHACUNGCAP ncc = new NHACUNGCAP();
                ncc.TEN_NCC = a;
                ncc.DIACHI_NCC = b;
                ncc.DT_NCC = c;
                data.NHACUNGCAPs.InsertOnSubmit(ncc);
                data.SubmitChanges();
                return RedirectToAction("DSNhaCungCap");
            }
        }
        [HttpGet]
        public ActionResult SuaNhaCungCap(int id)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                NHACUNGCAP res = data.NHACUNGCAPs.SingleOrDefault(a => a.MA_NCC == id);
                return View(res);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNhaCungCap(int id,FormCollection collection)
        {
            if (Session["TaiKhoanNV"] == null || Session["TaiKhoanNV"].ToString() == "")
            {
                return RedirectToAction("Login", "AdminShop");
            }
            else
            {
                string a = collection["TEN_NCC"];
                string b = collection["DIACHI_NCC"];
                string c = collection["DT_NCC"];

                NHACUNGCAP ncc = data.NHACUNGCAPs.Single(d => d.MA_NCC == id);
                ncc.TEN_NCC = a;
                ncc.DIACHI_NCC = b;
                ncc.DT_NCC = c;
                UpdateModel(ncc);
                data.SubmitChanges();
                return RedirectToAction("DSNhaCungCap");
            }
        }

    }


}