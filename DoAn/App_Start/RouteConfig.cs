using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "XemChiTietSanPham",
              url: "chi-tiet/{tensp}-{idsp}",
              defaults: new { action = "XemChitietSanPham", controller = "SanPham", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "XemtheoNSX",
              url: "san-pham/{tennsx}-{mansx}",
              defaults: new { action = "SanPham", controller = "SanPham", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "tintuc1",
              url: "tin-tuc/hay-tu-duy-nhu-mot-nguoi-tieu-dung-thong-thai",
              defaults: new { action = "News1", controller = "News", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "tintuc2",
              url: "tin-tuc/5-loai-thuc-pham-giup-tang-testosterone",
              defaults: new { action = "News2", controller = "News", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "tintuc3",
             url: "tin-tuc/7-cong-dung-cua-glutamine-khi-luyen-tap",
             defaults: new { action = "News3", controller = "News", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "tintuc4",
             url: "tin-tuc/tinh-thoi-gian-toi-uu-duong-chat-trong-co-the",
             defaults: new { action = "News4", controller = "News", id = UrlParameter.Optional }
         );


            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { action = "Index", controller = "Home", id = UrlParameter.Optional }
           );
        }
    }
}
