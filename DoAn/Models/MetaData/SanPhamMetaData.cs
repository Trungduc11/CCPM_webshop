using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    [MetadataTypeAttribute(typeof(SanPhamMetaData))]
    public partial class SanPham
    {  
       internal sealed class SanPhamMetaData
        {
            [DisplayName("Mã Sản Phẩm")]
            public int MaSP { get; set; }
            [DisplayName("Tên Sản Phẩm")]
            public string TenSP { get; set; }
            [DisplayName("Đơn Giá")]
            public Nullable<decimal> DonGia { get; set; }
            [DisplayName("Ngày Cập Nhật")]
            public Nullable<System.DateTime> NgayCapNhat { get; set; }
            [DisplayName("Mô Tả")]
            public string MoTa { get; set; }
            [DisplayName("Hình Ảnh")]
            public string HinhAnh { get; set; }
            [DisplayName("Số Lượng Tồn")]
            public Nullable<int> SoLuongTon { get; set; }
            [DisplayName("Mới")]
            public Nullable<int> Moi { get; set; }
            [DisplayName("Nhà Sản Xuất")]
            public Nullable<int> MaNSX { get; set; }
            [DisplayName("Mã Loại Sản Phẩm")]
            public Nullable<int> MaLoaiSP { get; set; }
            [DisplayName("Trạng thái")]
            public Nullable<bool> DaXoa { get; set; }
            [DisplayName("Tên Đường Dẫn")]
            public string TenDuongDan { get; set; }
            [DisplayName("Thành Phần Dinh Dưỡng")]
            public string Fact { get; set; }
        }
    }
}