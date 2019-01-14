using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopQuaTang.Models
{
    public class LoadAnh
    {
        dbQLShopQuaTangDataContext data = new dbQLShopQuaTangDataContext();
        public int MaSP { get; set; }
        public string AnhSP { get; set; }
        public int MaDM { get; set; }

 
    }
}