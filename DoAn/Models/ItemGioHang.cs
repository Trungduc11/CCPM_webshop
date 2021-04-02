using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
         public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public ItemGioHang(int inputMaSP)
        {
            using (BanHangEntities db = new BanHangEntities())
            {
                this.MaSP = inputMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == inputMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang()
        {

        }
    }
}