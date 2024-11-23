/*using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }
        public int MaSanPham { get; set; } // Khóa ngoại đến SanPham
        public string KhachHangId { get; set; } // Khóa ngoại đến AspNetUsers
        public int DiemDanhGia { get; set; }
        public string NoiDungDanhGia { get; set; }
        public DateTime NgayDanhGia { get; set; }

        // Quan hệ với SanPham và AspNetUsers
        public SanPham SanPham { get; set; }
        public ApplicationUser KhachHang { get; set; }
    }
}
*/