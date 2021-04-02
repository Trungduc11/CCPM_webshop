using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
namespace DoAn.Controllers
{
    public class GioHangController : Controller
    {
        BanHangEntities db = new BanHangEntities();
        // GET: GioHang
        //Lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstgioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstgioHang == null)
            {
                lstgioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstgioHang;
            }
            return lstgioHang;
        }

        //Thêm vào giỏ hàng
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //kiểm tra sản phẩm có trong DB hay không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trả về lỗi 404
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstgiohang = LayGioHang();
            //Sản phẩm tồn tại trong giỏ hàng
            ItemGioHang spcheck = lstgiohang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spcheck!=null)
            {
                if(sp.SoLuongTon<spcheck.SoLuong)
                {
                    return View();
                }    
                spcheck.SoLuong++;
                spcheck.ThanhTien = spcheck.SoLuong * spcheck.DonGia;
                return Redirect(strURL);
            }

            //Chưa có sản phẩm trong giỏ hàng
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                //thông báo hết hàng
                return View();
            }
            lstgiohang.Add(itemGH);
            return Redirect(strURL);
        }

        //Tính Tổng Số Lượng
        public double TinhTongSoLuong()
        {
            //Lấy giở hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang==null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }

        //Tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        //Hiển thị lên View
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        public ActionResult XemGioHang()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return View(lstGioHang);
        }
        [HttpGet]
        //Chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang(int? MaSP)
         {
            //kiểm tra session giỏ hàng đã tồn tại chưa?
            if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index","Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong DB không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra list giỏ hàng từ session
            List <ItemGioHang>  lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong giỏ hàng ko?
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spcheck==null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            //Lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            //Sản phẩm đã tồn tại trong giỏ hàng
            return View(spcheck);
        }

        //Xử lí cập nhật giở hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //kiểm tra số lượng tồn
            SanPham spcheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spcheck.SoLuongTon < itemGH.SoLuong)
            {
                //thông báo hết hàng
                return View();
            }
            //Lấy list<itemgiohang> trong sesion giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            //Lấy sản phẩm cần cập nhật
            ItemGioHang itemGioHangUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Cập nhật
            itemGioHangUpdate.SoLuong = itemGH.SoLuong;
            itemGioHangUpdate.ThanhTien = itemGioHangUpdate.SoLuong * itemGioHangUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }

        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int? MaSP)
        {
            //kiểm tra session giỏ hàng đã tồn tại chưa?
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong DB không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong giỏ hàng ko?
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa item trong database
            lstGioHang.Remove(spcheck);
            return RedirectToAction("XemGioHang");
        }

        //Đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khachhang = new KhachHang();
            if (Session["TaiKhoan"]==null)
            {
                //Thêm khách hàng vào bảng khách hàng khi khách hàng chưa có tài khoản
                khachhang = kh;
                db.KhachHangs.Add(khachhang);
                db.SaveChanges();
            }
            else
            {
                //Đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khachhang.TenKH = tv.HoTen;
                khachhang.DiaChi = tv.DiaChi;
                khachhang.MaThanhVien = tv.MaThanhVien;
                khachhang.Email = tv.Email;
                khachhang.SoDienThoai = tv.SoDienThoai;
                db.KhachHangs.Add(khachhang);
                db.SaveChanges();
            }    

            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khachhang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach( var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == item.MaSP);
                sp.SoLuongTon = sp.SoLuongTon - item.SoLuong;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("DatHangThanhCong");
        }

        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            //kiểm tra sản phẩm có trong DB hay không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trả về lỗi 404
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstgiohang = LayGioHang();
            //Sản phẩm tồn tại trong giỏ hàng
            ItemGioHang spcheck = lstgiohang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spcheck != null)
            {
                if (sp.SoLuongTon < spcheck.SoLuong)
                {
                    return View();
                }
                spcheck.SoLuong++;
                spcheck.ThanhTien = spcheck.SoLuong * spcheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }

            //Chưa có sản phẩm trong giỏ hàng
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                //thông báo hết hàng
                return View();
            }
            lstgiohang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }
        public ActionResult DatHangThanhCong()
        {
            return View();
        }
    }
}