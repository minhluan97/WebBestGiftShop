using Newtonsoft.Json.Linq;
using ShopQuaTang.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopQuaTang.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        const string ApiKey= "71FB19321910B92AAE4FF7D8A9E6CC";
        const string SecrectKey="359E3A63EE470DFE0A8F690B736AE2";

        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        public ActionResult Index()
        {
            return View();
        }

        //LoadThongTinDangNhap
        public ActionResult ThongTinKHPartial()
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            if (kh == null)
            {
                return PartialView();
            }
            else
            {
                var query = data.KHACHHANGs.SingleOrDefault(a => a.MA_KH == (kh.MA_KH));
                return PartialView(query);
            }
        }

        //DangKi
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(FormCollection collection, KHACHHANG kh)
        {
            var hoten_kh = collection["HOTEN_KH"];
            var sdt_kh = collection["SDT_KH"];
            var username_kh = collection["USERNAME_KH"];
            var password_kh = collection["PASSWORD_KH"];

            var res = data.KHACHHANGs.Where(a=>a.USERNAME_KH.Equals(username_kh)).ToList();
            if(res.Count!=0)
            {
                ModelState.AddModelError("","Trùng tên đăng nhập đã có");
                return RedirectToAction("DangKi", "KhachHang");
            }
            else
            {
                if (res.Count == 0)
                {
                    kh.HOTEN_KH = hoten_kh;
                    kh.SDT_KH = sdt_kh;
                    kh.USERNAME_KH = username_kh;
                    kh.CAPDO_KH = 1;
                    kh.TICHDIEM_KH = 10;
                    kh.PASSWORD_KH = EncryptMD5.MD5Hash(password_kh);
                    data.KHACHHANGs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("DangNhap");
        }

        //DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var namelogin = collection["USERNAME_KH"];
            var passlogin = collection["PASSWORD_KH"];
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n=>n.USERNAME_KH==namelogin && n.PASSWORD_KH==EncryptMD5.MD5Hash(passlogin));
            if (kh != null)
            {
                Session["TaiKhoan"] = kh;
                return RedirectToAction("DatHang","GioHang");
            }
            else
            {
                ViewBag.ThongBao = "Kiểm Tra Lại Username và Password";
            }
            return View();
        }

        //DangXuat
        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "QuaTang");
        }
        
        //DoiMatKhau
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection f)
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            string ma_kh = f["ma_kh"];
            string hoten_kh = f["hoten_kh"];
            string sdt_kh = f["sdt_kh"];
            string newpass = f["newpass"];
            string confirmpass = f["confirmpass"];
            if (String.Compare(newpass,confirmpass,true)==0)
            {
                var query = data.KHACHHANGs.Single(a => a.MA_KH == kh.MA_KH);
                query.PASSWORD_KH = EncryptMD5.MD5Hash(newpass);
                query.SDT_KH = sdt_kh;
                data.SubmitChanges();
            }
            else
            {
                ViewData["Loi"] = "Không khớp password";
            }
            ViewData["DoiThanhCong"] = "Đổi Thành Công!";
            return RedirectToAction("Index", "QuaTang");
        }

        //View thong tin khach hang
        public ActionResult ThongTinKhachHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            return View();
        }

        //Quen Mat Khau
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection collection)
        {
            string username_kh = collection["username_kh"];
            string sdt_kh = collection["sdt_kh"];
            string mess = "Mat%20khau%20cua%20tai%20khoan" + username_kh + "da%20duoc%20BestGiftShop%20dat%20lai%20la:%20";
            //gui request
            string URL = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone=" + sdt_kh+ "&Content=" + mess + username_kh+"@123"+"&ApiKey=" + ApiKey + "&SecretKey=" + SecrectKey + "&SmsType=4";
            //string URL = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone=" + phone + "&Content=" + message + "&Brandname=" + brandname + "&ApiKey=" + APIKey + "&SecretKey=" + SecretKey + "&IsUnicode=0&SmsType=2";
            ViewBag.GuiTN = URL;
            string result = SendGetRequest(URL);
            JObject ojb = JObject.Parse(result);
            int CodeResult = (int)ojb["CodeResult"];//trả về 100 là thành công
            int CountRegenerate = (int)ojb["CountRegenerate"];
            string SMSID = (string)ojb["SMSID"];//id của tin nhắn
            //update vao db
            KHACHHANG query = data.KHACHHANGs.Single(a => a.USERNAME_KH == username_kh);
            string resetpass = username_kh + "@123";
            query.PASSWORD_KH = EncryptMD5.MD5Hash(resetpass);
            data.SubmitChanges();
            return RedirectToAction("DangNhap","KhachHang");
        }
        private string SendGetRequest(string RequestUrl)
        {
            Uri address = new Uri(RequestUrl);
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.KeepAlive = false;
                request.Timeout = 15 * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    string result = reader.ReadToEnd();
                    result = result.Replace("</string>", "");
                    return result;
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        Console.WriteLine(
                            "The server returned '{0}' with the status code {1} ({2:d}).",
                            errorResponse.StatusDescription, errorResponse.StatusCode,
                            errorResponse.StatusCode);
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
            return null;
        }

        //xem lai lich giao dich
        public ActionResult LichSuGiaoDich()
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            if (kh == null || kh.ToString() == "")
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            else
            {
                var res = data.HOADONs.Where(a=>a.MA_KH==kh.MA_KH).ToList();
                return View(res);
            }
        }
        //Xem Chi Tiet Hoa Don
        public ActionResult XemChiTietHoaDon(int id)
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            if (kh == null || kh.ToString() == "")
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            else
            {
                var res = data.CT_HOADONs.Where(a => a.MA_HD == id).ToList(); ;
                return View(res);
            }
        }
    }
}