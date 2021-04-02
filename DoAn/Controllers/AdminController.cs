using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        BanHangEntities db = new BanHangEntities();
        [Authorize(Roles ="QuanTri")]
        public ActionResult Index()
        {
            return RedirectToAction("ThongKeTong");
        }
        [Authorize(Roles = "QuanTri")]
        //Xem doanh thu tông
        public ActionResult ThongKeTong()
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            ViewBag.SoLuongNguoiTruyCap = HttpContext.Application["SoLuongNguoiTruyCap"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu().ToString("#,##");
            ViewBag.ThongKeDonHang = ThongKeDonHang();
            ViewBag.ThongKeThanhVien = ThongKeThanhVien();
            return View();
        }
        //Xem doanh thu tháng
        [Authorize(Roles = "QuanTri")]
        public ActionResult DoanhThuThang(int? thang, int? nam)
         {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            if (ThongKeDoanhThuThang(thang,nam)==0)
            {
                ViewBag.DoanhThuTheoThang = 0;
            }    
            else ViewBag.DoanhThuTheoThang = ThongKeDoanhThuThang(thang, nam).ToString("#,##");
            if (ThongKeDonHangTheoThang(thang, nam) == 0)
            {
                ViewBag.ThongKeDonHangTheoThang = 0;
            }
            else ViewBag.ThongKeDonHangTheoThang = ThongKeDonHangTheoThang(thang, nam);
            if(thang == null)
            {
                ViewBag.DoanhThuTheoThang = ThongKeDoanhThuNam(nam).ToString("#,##");
            }    
            ViewBag.Thang = thang;
            ViewBag.Nam = nam;
            //Lọc đơn hàng theo năm được nhập
            if(thang == null)
            {
                return View(db.DonDatHangs.Where(p => p.NgayDat.Value.Year == nam));
            }
            //Đơn hàng lọc theo tháng năm
            var lst = db.DonDatHangs.Where(p => p.NgayDat.Value.Month == thang && p.NgayDat.Value.Year == nam);
            return View(lst);
        }
        public decimal ThongKeTongDoanhThu()
        {
            //Thống kê toàn bộ doanh thu
            decimal TongDoanhThu = decimal.Parse(db.ChiTietDonDatHangs.Sum(n => n.DonGia * n.SoLuong).Value.ToString());
            return TongDoanhThu/1000;
        }
        public decimal ThongKeDoanhThuThang(int ?thang, int ?nam)
        {
            //lst ra đơn hàng có tháng năm tương đương
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
            decimal TongTien = 0;
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien/1000;
        }
        public decimal ThongKeDoanhThuNam(int?nam)
        {
            //lst ra đơn hàng có  năm tương đương
            var lstDDH = db.DonDatHangs.Where(n =>n.NgayDat.Value.Year == nam);
            decimal TongTien = 0;
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien / 1000;
        }
        public double ThongKeDonHangTheoThang(int? thang, int?nam)
        {
            if(thang == null)
            {
                var ddh = db.DonDatHangs.Count(n => n.NgayDat.Value.Year == nam);
                return ddh;
            }    
            double ddh1 = db.DonDatHangs.Count(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
            return ddh1;
        }
        public double ThongKeDonHang()
        {
            //Đếm đơn đạt hàng
            double ddh = db.DonDatHangs.Count();
            return ddh;
        }

        public double ThongKeThanhVien()
        {
            //Đếm thành viên
            double sltv = db.ThanhViens.Count();
            return sltv;
        }
        //Xem chi tiết đơn  hàng
        [Authorize(Roles = "QuanTri")]
        public ActionResult ChiTiet(int? id)
        {
            ViewBag.MaDH = id;
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Kiểm tra id đơn hàng có hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra có tồn tại đơn hàng?
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách chi tiết đơn hàng
            var lstCTDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.lstCTDH = lstCTDH;
            return View(model);
        }
       
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(db!=null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}