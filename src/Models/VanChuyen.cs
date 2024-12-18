using System.ComponentModel.DataAnnotations;

namespace WebHM.Models
{
    public class VanChuyen
    {
        [Key]
        public int MaVanChuyen { get; set; } 

        [Required]
        [StringLength(100)]
        public string TenNhaVanChuyen { get; set; } 

        [Required]
        public decimal PhiVanChuyen { get; set; } 

        public string MoTa { get; set; } 
    }
}
