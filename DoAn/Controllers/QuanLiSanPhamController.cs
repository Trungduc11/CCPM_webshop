using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
using PagedList;

namespace DoAn.Controllers
{
    public class QuanLiSanPhamController : Controller
    {
        BanHangEntities db = new BanHangEntities();
        // GET: QuanLiSanPham
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult QuanLiSanPham(string sTuKhoa)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            if (sTuKhoa == null)
            {
                return View(db.SanPhams.Where(n => n.DaXoa == false));
            }
            //Tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            return View(lstSP.OrderBy(n => n.TenSP));
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult KQQLTimSP(string sTuKhoa)
        {
            return RedirectToAction("QuanLiSanPham", new { @sTuKhoa = sTuKhoa });
        }
        //Thêm sản phẩm Admin
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult TaoMoi()
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Load dropdownlist NCC
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX),"MaNSX","TenNSX");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai),"MaLoaiSP", "TenLoai");
            return View();
        }
        [Authorize(Roles = "QuanTri")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            //Load dropdownlist
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            var spthuc = db.SanPhams.SingleOrDefault(n => n.TenSP == sp.TenSP);
            if (spthuc!=null)
            {
                ViewBag.ten = "Sản phẩm đã có";
                return View();
            }    
            if(HinhAnh == null)
            {
                ViewBag.upload = "Vui lòng thêm hình ảnh sản phẩm";
                return View();
            }    
            //Kiểm tra hình ảnh tồn tại trong csdl chưa
            if(HinhAnh.ContentLength >0)
            {
                //Lấy tên hình ảnh
;               var filename = Path.GetFileName(HinhAnh.FileName);
                //Lấy hình ảnh chuyển vào thư mục
                var path = Path.Combine(Server.MapPath("~/Content/hinhanhsp"), filename);
                if(System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = filename;
                }
                db.SanPhams.Add(sp);
                db.SaveChanges();
            }
            return RedirectToAction("QuanLiSanPham");
        }
        //Sửa sản phẩm Admin
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Lấy sản phẩm cần chỉnh sửa
            if (id==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp==null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX","TenNSX",sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            return View(sp);
        }
        [Authorize(Roles = "QuanTri")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase HinhAnh)
        {
           if(ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.EntityState.Modified;
                if(HinhAnh != null)
                {
                    if (HinhAnh.ContentLength > 0)
                    {
                        //Lấy tên hình ảnh
                        ; var filename = Path.GetFileName(HinhAnh.FileName);
                        //Lấy hình ảnh chuyển vào thư mục
                        var path = Path.Combine(Server.MapPath("~/Content/hinhanhsp"), filename);
                        {
                            HinhAnh.SaveAs(path);
                            model.HinhAnh = filename;
                        }   
                    }
                }    
                db.SaveChanges();
                return RedirectToAction("QuanLiSanPham");
            }
            return View(model);
        }
        //Xóa sản phẩm
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //Lấy sản phẩm cần chỉnh xóa
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
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        [Authorize(Roles = "QuanTri")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            SanPham model = db.SanPhams.SingleOrDefault(n=>n.MaSP == id);
            if(model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLiSanPham");
        }
    }
}