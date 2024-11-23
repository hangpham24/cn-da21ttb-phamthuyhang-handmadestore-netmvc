using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class GioHang
    {
        [Key]
        public int MaGioHang { get; set; }
        public string KhachHangId { get; set; } // Khóa ngoại đến AspNetUsers
        public int MaSanPham { get; set; } // Khóa ngoại đến SanPham
        public int SoLuong { get; set; }

        // Quan hệ với AspNetUsers và SanPham
        public ApplicationUser KhachHang { get; set; }
        public SanPham SanPham { get; set; }
    }
}
