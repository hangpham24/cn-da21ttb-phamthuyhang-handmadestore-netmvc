using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }

        [Required]
        [StringLength(255)]
        public string TenSanPham { get; set; }

        public decimal Gia { get; set; }

        public string MoTa { get; set; }

        public string HinhAnh { get; set; }

        public int MaDanhMuc { get; set; }
        public DanhMuc DanhMuc { get; set; }

        public string NguoiBanId { get; set; }
        public ApplicationUser NguoiBan { get; set; }

        // Các thuộc tính ICollection để xác định quan hệ một-nhiều
/*        public ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();*/
        public ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();
        public ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
        public ICollection<ThongKeDoanhSo> ThongKeDoanhSos { get; set; } = new List<ThongKeDoanhSo>();

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
