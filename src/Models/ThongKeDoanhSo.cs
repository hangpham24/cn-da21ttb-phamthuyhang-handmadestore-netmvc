using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class ThongKeDoanhSo
    {
        [Key]
        public string NguoiBanId { get; set; } // Khóa ngoại đến AspNetUsers
        public int MaSanPham { get; set; } // Khóa ngoại đến SanPham
        public DateTime NgayBan { get; set; }
        public int SoLuongDaBan { get; set; }
        public decimal TongDoanhThu { get; set; }

        // Quan hệ với AspNetUsers và SanPham
        public ApplicationUser NguoiBan { get; set; }
        public SanPham SanPham { get; set; }
    }
}
