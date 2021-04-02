using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace DoAn.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        BanHangEntities db = new BanHangEntities();
        //Tạo 2 View sản phẩm, hiển thị sản phẩm
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        //Xây dựng trang sản phẩm chi tiết
        public ActionResult XemChitietSanPham (int? idsp, string tensp)
        {
            if(idsp == null)
            {
                //Báo lỗi không có tham số truyềN vào
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //Truy suất csdl lấy ra sản phẩm tương ứng với tham số id truyền vào
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == idsp && n.DaXoa == false);
            if(sp==null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }    
            return PartialView(sp);
        }

        //Xây dựng trang sản phẩm theo NSX
        public ActionResult SanPham(int? mansx, string tennsx,int ?page)
        {
            //Load sản phẩm theo NSX
            var lstsp = db.SanPhams.Where(n => n.MaNSX == mansx);
            if(lstsp.Count()==0)
            {
                return HttpNotFound();
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 9;
            int pagenumber = (page ?? 1);
            ViewBag.MaNSX = mansx;
            ViewBag.TenNSX = tennsx;
            return PartialView(lstsp.OrderBy(n => n.MaSP).ToPagedList(pagenumber,pagesize));
        }

        //Sản phẩm tương tự
        public ActionResult SanPhamTuongTu(int? loaisp,int? id)
        {
            var lstsptt = db.SanPhams.Where(n => n.MaLoaiSP == loaisp && n.MaSP!=id).ToList().OrderBy(n=>n.TenSP);
            return PartialView(lstsptt);
        }

        //sản phẩm theo danh mục
        public ActionResult SanPhamDanhMuc(int? maloai, int? page)
        {
            
            //Load sản phẩm theo MaLoai
            var lstsp = db.SanPhams.Where(n => n.MaLoaiSP == maloai);
            if (lstsp.Count() == 0)
            {
                return HttpNotFound();
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 9;
            int pagenumber = (page ?? 1);
            ViewBag.MaLoai = maloai;
            return PartialView(lstsp.OrderBy(n => n.MaLoaiSP).ToPagedList(pagenumber, pagesize));
        }
    }
}