using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace DoAn.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        BanHangEntities db = new BanHangEntities();

        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            ViewBag.Ten = sTuKhoa;
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pagesize = 6;
            int pagenumber = (page ?? 1);
            ViewBag.TuKhoa = sTuKhoa;
            //Tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(pagenumber, pagesize));
        }
        [HttpPost]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            //Gọi về hàm get tìm kiếm
            return RedirectToAction("KetQuaTimKiem",new { @sTuKhoa = sTuKhoa});
        }
    }
}