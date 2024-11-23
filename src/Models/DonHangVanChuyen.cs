using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class DonHangVanChuyen
    {
        [Key]
        public int Id { get; set; }
        public int MaDonHang { get; set; }
        public DonHang DonHang { get; set; }
        public int MaVanChuyen { get; set; }
        public VanChuyen VanChuyen { get; set; }
        public string TrangThai { get; set; } // VD: Đang giao, Hoàn tất, Hủy
        public DateTime NgayCapNhat { get; set; }
    }
}
