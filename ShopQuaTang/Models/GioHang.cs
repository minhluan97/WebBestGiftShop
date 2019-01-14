using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopQuaTang.Models
{
    public class GioHang
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        public int MaSP { get; set; }
        public int MaDM { get; set; }
        public string TenSP { get; set; }
        public float DonGiaSP { get; set; }
        public string AnhSP { get; set; }
        public int SoLuongSP { get; set; }
        public float ThanhTienHD {
            get {return SoLuongSP * DonGiaSP;}
        }

        public GioHang(int masp)
        {
            MaSP = masp;
            SANPHAM sp = data.SANPHAMs.Single(n => n.MA_SP == masp);
            TenSP = sp.TEN_SP;
            MaDM = int.Parse(sp.MA_DM.ToString());
            AnhSP = sp.AVA_SP;
            DonGiaSP = float.Parse(sp.GIA_SP.ToString());
            SoLuongSP = 1;
        }


    }
}