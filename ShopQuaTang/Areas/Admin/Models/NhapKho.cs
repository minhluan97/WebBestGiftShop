using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopQuaTang.Areas.Admin.Models;
namespace ShopQuaTang.Areas.Admin.Models
{
    public class NhapKho
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        public int MaSP { get; set; }
        public int MaDM { get; set; }
        public string TenSP { get; set; }
        public float DonGiaSPNhap { get; set; }
        public string AnhSP { get; set; }
        public int SoLuongSPNhap { get; set; }
        public float ThanhTienPN
        {
            get { return SoLuongSPNhap * DonGiaSPNhap; }
        }
        public NhapKho(int masp)
        {
            MaSP = masp;
            SANPHAM sp = data.SANPHAMs.Single(n => n.MA_SP == masp);
            TenSP = sp.TEN_SP;
            MaDM = int.Parse(sp.MA_DM.ToString());
            AnhSP = sp.AVA_SP;
            if(MaDM==4||MaDM==10 ||MaDM==11||MaDM==12)
            {
                DonGiaSPNhap = float.Parse(sp.GIA_SP.ToString()) - (float)25000;
            }
            else
            {
                DonGiaSPNhap = float.Parse(sp.GIA_SP.ToString()) - (float)10000;
            }
            SoLuongSPNhap = 1;
        }
    }
}