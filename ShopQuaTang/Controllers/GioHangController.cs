using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShopQuaTang.Models;
using WebApplication2;
namespace ShopQuaTang.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();

        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang==null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }
        public ActionResult ThemGioHang(int masp,string strURL)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.Find(n=>n.MaSP==masp);
            if (sp == null)
            {
                sp = new GioHang(masp);
                listGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.SoLuongSP++;
                return Redirect(strURL);
            }
        }
        public ActionResult XoaGioHang(int masp)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n => n.MaSP == masp);
            if (sp != null)
            {
                listGioHang.RemoveAll(a => a.MaSP == masp);
                return RedirectToAction("GioHang");
            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index","QuaTang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int masp,FormCollection fc)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n => n.MaSP == masp);
            if (sp != null)
            {
                sp.SoLuongSP = int.Parse(fc["slsp"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        private int TongSoLuong()
        {
            int tongsl = 0;
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang != null)
            {
                tongsl = listgiohang.Sum(a=>a.SoLuongSP);
            }
            return tongsl;
        }
        private float TongTienHD()
        {
            float tongtien = 0;
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang != null)
            {
                tongtien = listgiohang.Sum(a => a.ThanhTienHD);
            }
            return tongtien;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GioHang()
        {
            List<GioHang> listGioHang = LayGioHang();
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index","QuaTang");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTienHD(); ;
            return View(listGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTienHD();
            return PartialView();
        }
        public  ActionResult XoaTatCaGioHang()
        {
            List<GioHang> listGioHang = LayGioHang();
            listGioHang.Clear();
            return RedirectToAction("Index","QuaTang");
        }
        //Dat hang
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null|| Session["TaiKhoan"].ToString()=="")
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "QuaTang");
            }
            List< GioHang > listGioHang= LayGioHang();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            int sl = TongSoLuong();
            float tt = TongTienHD();
            float gg=0;
            if ((kh.CAPDO_KH==1 && (tt>1000000&&tt<=2000000) && kh.TICHDIEM_KH<=100) ||(kh.CAPDO_KH==1 &&sl>5))
            {
                gg = (float)0.07;
                tt = tt * gg;
                kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)12;
                kh.CAPDO_KH = 2;
            }
            else
            {
                if((kh.CAPDO_KH==1 && (tt>=1500000)) || (kh.CAPDO_KH==1 && kh.TICHDIEM_KH > 200) || (kh.CAPDO_KH == 1 && tt>=1300000 && sl > 10))
                {
                    gg = (float)0.10;
                    tt = tt * gg;
                    kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)15;
                    kh.CAPDO_KH = 2;
                }
                else
                {
                    if (kh.CAPDO_KH == 2 && (tt >= 2000000 && tt <= 3000000) && kh.TICHDIEM_KH <= 250)
                    {
                        gg = (float)0.12;
                        tt = tt * gg;
                        kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)20;
                        kh.CAPDO_KH = 3;
                    }
                    else
                    {
                        if ((kh.CAPDO_KH == 2 && (tt >= 2500000)) || (kh.CAPDO_KH == 2 && (kh.TICHDIEM_KH > 250 && kh.TICHDIEM_KH < 300)))
                        {
                            gg = (float)0.12;
                            tt = tt * gg;
                            kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)25;
                            kh.CAPDO_KH = 3;
                        }
                        else
                        {
                            if (kh.CAPDO_KH == 3 && (tt >= 3000000 && tt <= 3500000) && (kh.TICHDIEM_KH >= 250 && kh.TICHDIEM_KH < 325))
                            {
                                gg = (float)0.15;
                                tt = tt * gg;
                                kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)30;
                                kh.CAPDO_KH = 4;
                            }
                            else
                            {
                                if ((kh.CAPDO_KH == 3 && (tt >= 3250000)) || (kh.CAPDO_KH == 3 && (kh.TICHDIEM_KH >= 325 && kh.TICHDIEM_KH < 400)))
                                {
                                    gg = (float)0.18;
                                    tt = tt * gg;
                                    kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)35;
                                    kh.CAPDO_KH = 4;
                                }
                                else
                                {
                                    if ((kh.CAPDO_KH == 4 && (tt > 3250000&&tt<4000000)) || (kh.CAPDO_KH == 3 && (kh.TICHDIEM_KH >= 325 && kh.TICHDIEM_KH < 400) && tt > 3750000))
                                    {
                                        gg = (float)0.25;
                                        tt = tt * gg;
                                        kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)50;
                                    }
                                    else
                                    {
                                        gg = (float)0.05;
                                        tt = tt * gg;
                                        kh.TICHDIEM_KH = kh.TICHDIEM_KH + (int)10;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            ViewBag.TongSL = TongSoLuong();
            ViewBag.SauGiamGia = (float)TongTienHD() - tt;
            ViewBag.GiamGia = gg;
            ViewBag.CapDo = kh.CAPDO_KH;
            ViewBag.TichDiem = kh.TICHDIEM_KH;
            return View(listGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            HOADON hd = new HOADON();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> listGH = LayGioHang();
            var ngaygiaohang = String.Format("{0:MM/dd/yyyy}", collection["ngaygiaohang"]);
            string hotenKN = collection["hotenkn"];
            string sdtKN = collection["sdtkn"];
            string diachiKN = collection["diachikn"];
            string saugiamgia = collection["saugiamgia"];
            byte capdo = byte.Parse(collection["capdo"]);
            int tichdiem = int.Parse(collection["tichdiem"]);

            //cap nhat khach hang
            var query = data.KHACHHANGs.Single(a => a.MA_KH == kh.MA_KH);
            query.CAPDO_KH = capdo;
            query.TICHDIEM_KH = tichdiem;
            data.SubmitChanges();

            //Them hoa don
            hd.MA_KH = kh.MA_KH;
            hd.NGAYLAP_HD = DateTime.Now;
            hd.TONGTIEN_HD = float.Parse(saugiamgia.ToString());
            hd.NGAYGIAOHANG = DateTime.Parse(ngaygiaohang);
            hd.STATUS_HD = false;
            hd.STATUS_GH = false;
            hd.HOTEN_KN = hotenKN;
            hd.SDT_KN = sdtKN;
            hd.DIACHI_KN = diachiKN;
           
            //thanh toan truc tuyen collection["XacNhanHoaDon" == true
            //value true == onepay false chuyen khoan
            string mathanhtoantructuyen = DateTime.Now.Ticks.ToString();
            if (string.Compare(collection["XacNhanHoaDon"], "OnePay") ==0)
            {
                string SECURE_SECRET = OnepayCode.SECURE_SECRET;// HAO : CAN THANH MA THAT TRONG APP CODE
                //// KHOI TAO LOP THU VIEN  VA GAN GIA TRI CAC THAM SO
                VPCRequest conn = new VPCRequest(OnepayCode.VPCRequest);
                conn.SetSecureSecret(SECURE_SECRET);
                //add digital order field
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "vn");
                conn.AddDigitalOrderField("vpc_Version", "2");
                conn.AddDigitalOrderField("vpc_Command", "pay");
                conn.AddDigitalOrderField("vpc_Merchant", OnepayCode.Merchant);
                conn.AddDigitalOrderField("vpc_AccessCode", OnepayCode.AccessCode);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", mathanhtoantructuyen);
                conn.AddDigitalOrderField("vpc_OrderInfo", mathanhtoantructuyen);
                conn.AddDigitalOrderField("vpc_Amount", (TongTienHD() * 100).ToString());
                conn.AddDigitalOrderField("vpc_Currency", "VND");
                conn.AddDigitalOrderField("vpc_ReturnURL",OnepayCode.ReturnURL);
                conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
                conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
                conn.AddDigitalOrderField("vpc_SHIP_City", "");
                conn.AddDigitalOrderField("vpc_SHIP_Country", "");
                conn.AddDigitalOrderField("vpc_Customer_Phone", "");
                conn.AddDigitalOrderField("vpc_Customer_Email", "");
                conn.AddDigitalOrderField("vpc_Customer_Id", "");
                conn.AddDigitalOrderField("vpc_TicketNo", Request.UserHostAddress);
                string ketQua = "";
                string url = conn.Create3PartyQueryString();
                ketQua = url;

                hd.STATUS_TT = true;
                data.HOADONs.InsertOnSubmit(hd);
                data.SubmitChanges();
                //thêm tblCTHoaDon
                foreach (var item in listGH)
                {
                    SANPHAM sp = new SANPHAM();
                    CT_HOADON cthd = new CT_HOADON();

                    //cap nhat sl ton kho
                    var capnhatSL = data.SANPHAMs.Single(a => a.MA_SP == item.MaSP);
                    if (capnhatSL.SL_TON <= item.SoLuongSP)
                    {
                        cthd.SL = sp.SL_TON;
                        capnhatSL.SL_TON = 0;
                    }
                    else
                    {
                        cthd.SL = item.SoLuongSP;
                        capnhatSL.SL_TON = capnhatSL.SL_TON - item.SoLuongSP;
                    }
                    cthd.MA_HD = hd.MA_HD;
                    cthd.MA_SP = item.MaSP;
                    cthd.SL = item.SoLuongSP;
                    cthd.DONGIA = capnhatSL.GIA_SP;
                    cthd.THANHTIEN_HD = item.SoLuongSP * cthd.DONGIA;
                    UpdateModel(sp);
                    //ghi ct hoa don
                    data.CT_HOADONs.InsertOnSubmit(cthd);
                }
                data.SubmitChanges();
                Session["GioHang"] = null;
                return Redirect(ketQua);
            }
            else
            {
                hd.STATUS_TT = false;
                data.HOADONs.InsertOnSubmit(hd);
                data.SubmitChanges();
                //them ct hoa don
                foreach (var item in listGH)
                {
                    SANPHAM sp = new SANPHAM();
                    CT_HOADON cthd = new CT_HOADON();

                    //cap nhat sl ton kho
                    var capnhatSL = data.SANPHAMs.Single(a => a.MA_SP == item.MaSP);
                    if (capnhatSL.SL_TON <= item.SoLuongSP)
                    {
                        cthd.SL = sp.SL_TON;
                        capnhatSL.SL_TON = 0;
                    }
                    else
                    {
                        cthd.SL = item.SoLuongSP;
                        capnhatSL.SL_TON = capnhatSL.SL_TON - item.SoLuongSP;
                    }
                    cthd.MA_HD = hd.MA_HD;
                    cthd.MA_SP = item.MaSP;
                    cthd.SL = item.SoLuongSP;
                    cthd.DONGIA = capnhatSL.GIA_SP;
                    cthd.THANHTIEN_HD = item.SoLuongSP * cthd.DONGIA;
                    data.SubmitChanges();
                    //ghi ct hoa don
                    data.CT_HOADONs.InsertOnSubmit(cthd);

                }
                data.SubmitChanges();
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}