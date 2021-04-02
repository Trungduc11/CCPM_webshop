using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class QuanLiThanhVienController : Controller
    {
        BanHangEntities db = new BanHangEntities();
        // GET: QuanLiThanhVien
        [Authorize(Roles = "QuanTri")]
        public ActionResult QuanLiThanhVienAdmin()
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            var lst = db.ThanhViens.Where(n => n.MaLoaiTV == 1);
            return View(lst);
        }
        [Authorize(Roles = "QuanTri")]
        public ActionResult QuanLiThanhVienUser()
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            var lst = db.ThanhViens.Where(n => n.MaLoaiTV == 2);
            return View(lst);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult ChinhSuaTV(int? id)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Lấy sản phẩm cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai", tv.MaLoaiTV);
            return View(tv);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult ChinhSuaTV(ThanhVien model)
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuanLiThanhVienAdmin");
            }
            return View(model);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult ThemThanhVien()
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Load dropdownlist Loai Thanh Vien
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            ThanhVien tv = new ThanhVien();
            return View(tv);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult ThemThanhVien(ThanhVien tv)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Load dropdownlist Loai Thanh Vien
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            var tvthuc = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == tv.TaiKhoan);
            if (tvthuc != null)
            {
                ViewBag.ten = "Tài khoản đã tồn tại";
                return View();
            }
            if (ModelState.IsValid)
            {
                ViewBag.ThongBao = "Thêm thành công";
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                System.Threading.Thread.SpinWait(3);
                Response.Redirect("QuanLiThanhVienAdmin");
            }
            else
            {
                ViewBag.ThongBao = "Thêm thất bại";
            }           
            return View(tv);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Lấy thành viên cần chỉnh xóa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai", tv.MaLoaiTV);
            return View(tv);
        }
        [Authorize(Roles = "QuanTri")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            ThanhVien model = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.ThanhViens.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLiThanhVienAdmin");
        }

    }
}