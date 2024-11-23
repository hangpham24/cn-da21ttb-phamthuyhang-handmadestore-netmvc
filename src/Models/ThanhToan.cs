using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class ThanhToan
    {
        [Key]
        public int MaThanhToan { get; set; }
        public int MaDonHang { get; set; } // Khóa ngoại đến AspNetUsers
        public decimal SoTien { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string PhuongThuc { get; set; } // Ví dụ: thẻ tín dụng, PayPal, v.v.

        // Quan hệ với AspNetUsers
        public DonHang DonHang { get; set; }
    }
}
