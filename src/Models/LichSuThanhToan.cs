using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class LichSuThanhToan
    {
        [Key]
        public int MaLichSu { get; set; }
        public int MaThanhToan { get; set; } // Khóa ngoại đến ThanhToan
        public DateTime NgayGiaoDich { get; set; }
        public decimal SoTien { get; set; }
        public string TrangThai { get; set; } // Thành công, thất bại, đang xử lý

        // Quan hệ với ThanhToan
        public ThanhToan ThanhToan { get; set; }
    }
}
