using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopQuaTang.Models;
using PagedList;
using PagedList.Mvc;
namespace ShopQuaTang.Controllers
{
    public class QuaTangController : Controller
    {
        // GET: QuaTang
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();

        private List<SANPHAM> TopSPMoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NGAYCAPNHAT_SP).OrderByDescending(a=>a.GIA_SP).Take(count).ToList();
        }

        public ActionResult Index()
        {
            var newproduct = TopSPMoi(6);
            return View(newproduct);
        }
        public ActionResult TinMoiNhatPartial()
        {
            var topnews = data.TINTUCs.OrderByDescending(a => a.NGAYDANG_TIN).Take(4).ToList();
            return PartialView(topnews);
        }
        //Load voi view cua trang chu
        public ActionResult DanhMuc()
        {
            var category = from dm in data.DANHMUCs select dm;
            return PartialView(category);
        } 
        //Load voi view cua cac trang khac trang chu
        public ActionResult DanhMucSP()
        {
            var category = from dm in data.DANHMUCs select dm;
            return PartialView(category);
        }
        public ActionResult QuaTangTheoDanhMuc(int id,int?page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var product= from sp in data.SANPHAMs where sp.MA_DM==id select sp;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTietSanPham(int id)
        {
            var detailproduct = from sp in data.SANPHAMs
                                where sp.MA_SP == id
                                select sp;
            return View(detailproduct.Single());
        }
        public ActionResult LienHe()
        {
            return View();
        } 
        public ActionResult LoadAnh(int id)
        {
            var res = from ha in data.HINHANH_SPs where ha.MA_SP == id select ha;
            SANPHAM sp = data.SANPHAMs.First(m => m.MA_SP ==id);
            ViewBag.MaDM = sp.MA_DM;
            return PartialView(res);
        }
        public ActionResult SanPhamLienQuan(int id)
        {
            var res = from sp in data.SANPHAMs.OrderBy(a=>a.GIA_SP).Where(a => a.MA_DM == id).Take(3) select sp;
            return PartialView(res);
        }
        public ActionResult SanPhamCungDanhMuc(int id)
        {
            var res = from sp in data.SANPHAMs.OrderBy(a => a.GIA_SP).OrderByDescending(a=>a.NGAYCAPNHAT_SP).Where(a => a.MA_DM == id).Take(6) select sp;
            return PartialView(res);
        }
        public ActionResult SanPham()
        {
            var res = from dm in data.DANHMUCs.Take(4) select dm;
            return View(res);
        }

        public ActionResult TimKiemSanPham()
        {
            return PartialView();
        }

        public ActionResult KetQuaTimKiem(FormCollection f,int ? page)
        {
            string tkiem = f["searchbox"];
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var s = from sp in data.SANPHAMs select sp;
            if (!String.IsNullOrEmpty(tkiem))
            {
                s=s.Where(sp => sp.TEN_SP.Contains(tkiem));
            }

            return View(s.ToPagedList(pageNum, pageSize));
        }
        
    }
}