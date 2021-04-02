using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            [DisplayName("Mã thành viên")]
            public int MaThanhVien { get; set; }

            [DisplayName("Tài khoản")]
            [Required(ErrorMessage = "{0} không dược để trống")]
            public string TaiKhoan { get; set; }

            [DisplayName("Mật khẩu")]
            [Required(ErrorMessage = "{0} không dược để trống")]
            [RegularExpression("^.*(?=.{8,})(?=.*[\\d])(?=.*[A-Z]).*$", ErrorMessage = "{0} phải có ít nhất 8 kí tự trong đó có ít nhất 1 chữ hoa và 1 số từ 0-9")]
            public string MatKhau { get; set; }

            [DisplayName("Họ tên")]
            public string HoTen { get; set; }

            [DisplayName("Địa chỉ")]
            public string DiaChi { get; set; }

            [DisplayName("Email")]
            [RegularExpression("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "{0} không hợp lệ")]
            public string Email { get; set; }

            [DisplayName("Số điện thoại")]
            public string SoDienThoai { get; set; }

            [DisplayName("Mã loại TV")]
            public Nullable<int> MaLoaiTV { get; set; }

        }
    }
}