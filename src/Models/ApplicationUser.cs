using Microsoft.AspNetCore.Identity;

namespace WebHM.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Thuộc tính chung cho tất cả người dùng
        public string? HoTen { get; set; } // Tên đầy đủ của người dùng
        public string? DiaChi { get; set; } // Địa chỉ liên hệ
        public DateTime? NgaySinh { get; set; } // Ngày sinh
        public DateTime? NgayTaoTaiKhoan { get; set; } // Ngày tạo tài khoản
        public string? UrlAnhDaiDien { get; set; } // URL ảnh đại diện của người dùng

        // Các thuộc tính cho người dùng Seller
        public string? TenCuaHang { get; set; } // Tên cửa hàng
        public string? DiaChiCuaHang { get; set; } // Địa chỉ cửa hàng
        public string? MaSoThue { get; set; } // Mã số thuế
        public DateTime? NgayDangKyCuaHang { get; set; } // Ngày đăng ký cửa hàng
    }
}
