using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }

        // Quan hệ một-nhiều với sản phẩm
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
