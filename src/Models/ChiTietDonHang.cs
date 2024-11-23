using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTiet { get; set; }
        public int MaDonHang { get; set; } // Khóa ngoại đến DonHang
        public int MaSanPham { get; set; } // Khóa ngoại đến SanPham
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }

        // Quan hệ với DonHang và SanPham
        public DonHang DonHang { get; set; }
        public SanPham SanPham { get; set; }
    }
}
