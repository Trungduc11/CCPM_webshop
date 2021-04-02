using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class QuanLiNhapHangController : Controller
    {
        BanHangEntities db = new BanHangEntities();
        // GET: QuanLiNhapHang
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult NhapHang(int?id)
        {
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult NhapHang(Phieunhap model,ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //Kiểm tra đầu vào
            model.DaXoa = false;
            model.NgayNhap = DateTime.Now;
            db.Phieunhaps.Add(model);
            db.SaveChanges();
            ctpn.MaPN = model.MaPN;
            //Cập nhật số lượng tồn
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();
            return View(sp);
        }
        [Authorize(Roles = "QuanTri")]
        public ActionResult DSSPHetHang()
        {
            //Danh sách sản phẩm gần hết hàng có số lượng tồn <5
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            return View(spt);
        }
    }
}