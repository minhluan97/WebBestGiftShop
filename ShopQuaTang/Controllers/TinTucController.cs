using ShopQuaTang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopQuaTang.Controllers
{
    public class TinTucController : Controller
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        // GET: TinTuc
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChiTietTin(int id)
        {
            var detailproduct = from tt in data.TINTUCs
                                where tt.MA_TIN == id
                                select tt;
            return View(detailproduct.Single());
        }

        public ActionResult PostCommentPartial()
        {
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            return PartialView();
        }
    }
}