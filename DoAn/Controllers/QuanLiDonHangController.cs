using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
using System.Net.Mail;

namespace DoAn.Controllers
{
    public class QuanLiDonHangController : Controller
    {
        BanHangEntities db = new BanHangEntities();
        // GET: QuanLiDonHang
        public string chuyen(bool a)
        {
            if (a == false) return "Chưa";
            else return "Rồi";
        }
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult ChuaThanhToan(int? sTuKhoa)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Lấy danh sách các đơn hàng chưa được duyệt
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false && n.TinhTrangGiaoHang == false).OrderBy(n => n.NgayDat);
            if (sTuKhoa == null)
            {
                return View(lst);
            }
            //Duyệt theo Mã đơn hàng của các đơn hàng chưa được duyệt
            var lsttim = db.DonDatHangs.Where(n => n.DaThanhToan == false && n.TinhTrangGiaoHang == false && n.MaDDH == sTuKhoa).OrderBy(n => n.NgayDat);
            return View(lsttim);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult TimChuaThanhToan(int? sTuKhoa)
        {
            return RedirectToAction("ChuaThanhToan", new { @sTuKhoa = sTuKhoa });
        }
        public ActionResult ChuaGiao()
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Lấy danh sách các đơn hàng chưa giao
            var lst = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan==true).OrderBy(n => n.NgayGiao);
            return View(lst);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult DaGiaoDaThanhToan(int? sTuKhoa)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Lấy danh sách các đơn hàng đã thanh toán đã giao
            var lst = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan==true).OrderBy(n => n.NgayGiao);
            if(sTuKhoa == null)
            {
                return View(lst);
            }    
            var lsttim = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true && n.MaDDH==sTuKhoa).OrderBy(n => n.NgayGiao);
            return View(lsttim);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult TimDaThanhToan(int? sTuKhoa)
        {
            return RedirectToAction("DaGiaoDaThanhToan", new { @sTuKhoa = sTuKhoa });
        }
        [Authorize(Roles = "QuanTri")]
        [HttpGet]
        public ActionResult DuyetDonHang(int? id,string email)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Kiểm tra id đơn hàng có hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra có tồn tại đơn hàng?
            if(model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách chi tiết đơn hàng
            var lstCTDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.lstCTDH = lstCTDH;
            return View(model);
        }
        [Authorize(Roles = "QuanTri")]
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh, KhachHang kh)
        {
            var spt = db.SanPhams.Where(n => n.SoLuongTon < 5 && n.DaXoa == false);
            ViewBag.SoLuongTon = spt.Count();
            //Truy vấn dữ liệu đơn hàng
            DonDatHang ddhup = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhup.DaThanhToan = ddh.DaThanhToan;
            ddhup.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            var lstCTDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.lstCTDH = lstCTDH;
           if (ddhup.DaThanhToan == true && ddhup.TinhTrangGiaoHang == true)
            {
                GuiMail("Thông báo", kh.Email, "0Vfitness0@gmail.com", "tanvu321654987","<div>Chào bạn "+kh.TenKH+"!</div>" +
                    "</br><div>Đơn hàng của bạn đang trong quá trình xử lí. Chúng tôi sẽ xem xét và liên hệ với bạn trong thời gian sớm nhất</div>" +
                    "</br>");
            }    
            return RedirectToAction("DaGiaoDaThanhToan",ddhup);
        }

        //Gửi  mail
        public void GuiMail(string Tilte, string ToEmail, string FromEmail, string Password,string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ người nhận
            mail.From = new MailAddress(ToEmail);//Đia chỉ người gửi
            mail.Subject = Tilte; //Tiêu đề
            mail.Body = Content; // Nội dung mail
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(FromEmail, Password);//Tài khoản Password người gửi
            smtp.EnableSsl = true; //Kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail); //Gửi mail đi
        }
    }
}