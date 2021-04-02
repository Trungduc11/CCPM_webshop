using DoAn.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Security;

namespace DoAn.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        BanHangEntities db = new BanHangEntities();
        public ActionResult Index()
        { 
            //Tạo viewbag lấy list sản phẩm tăng cân tăng cơ thuộc dòng mới
            var lstmoi = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            //Gán vào viewbag
            ViewBag.ListMoi = lstmoi;

            //Tạo viewbag lấy list sản phẩm tăng năng lượng mới
            var lstmoi1 = db.SanPhams.Where(n => n.MaLoaiSP == 5 && n.Moi == 1 && n.DaXoa == false);
            //Gán vào viewbag
            ViewBag.ListMoi1 = lstmoi1;
            return View();
        }
        //Load menu động
        public ActionResult MenuPartial()
        {
            var lstmenu = db.NhaSanXuats.ToList();
            return PartialView(lstmenu);
        }
        //Load danh mục loại sản phẩm
        public ActionResult DanhMuc()
        {
            var lstloai = db.LoaiSanPhams.ToList();  
            return PartialView(lstloai);
        }
        //Đăng kí
        [HttpGet]
        public ActionResult DangKy()
        {
            ThanhVien tv = new ThanhVien();
            return View(tv);
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {

            if (ModelState.IsValid)
            {
                ViewBag.ThongBao = "Thêm thành công";
                db.ThanhViens.Add(tv);
                db.SaveChanges();
            }
            else
            {
                ViewBag.ThongBao = "Thêm thất bại";
            }
            return View(tv);
        }
       
        //Đăng xuất
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpPost]
        //Đăng nhập
        public ActionResult DangNhap(FormCollection f)
        {
            ////kiểm tra tên đăng nhập và mật khẩu
            //string Taikhoan = f["TenDangNhap"].ToString();
            //string MatKhau = f["MatKhau"].ToString();

            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == Taikhoan && n.MatKhau == MatKhau);
            //if (tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    return Content("<script>window.location.reload()</script>");
            //}
            //return Content("Tên đăng nhập hoặc mật khẩu không đúng!");
            string Taikhoan = f["TenDangNhap"].ToString();
            string MatKhau = f["MatKhau"].ToString();
            //Truy vấn kiểm tra đăng nhập lấy thông tin thành viên
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == Taikhoan && n.MatKhau == MatKhau);
            if(tv!=null)
            {
                Session["TaiKhoan"] = tv;
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //Duyệt lst
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);// Loại bỏ dấu phẩy
                PhanQuyen(tv.MaThanhVien.ToString(), Quyen);
                return Content("<script>window.location.reload()</script>");
            }
            return Content("Tên đăng nhập hoặc mật khẩu không đúng!");
        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                                       TaiKhoan,//tài khoản
                                                       DateTime.Now, //thời gian bắt đàu
                                                       DateTime.Now.AddHours(3),//thời gian kết thúc (3h)
                                                       false,
                                                       Quyen,// Chuỗi quyền được cấp
                                                       FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);                                          
        }

        //tạo trang ngăn chặn quyền truy cập
        public ActionResult LoiPhanQuyen()
        { 
            return PartialView();
        }
    }
}