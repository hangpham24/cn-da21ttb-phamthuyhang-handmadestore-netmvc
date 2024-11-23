namespace WebHM.Models.ViewModel
{
	public class ThanhToanViewModel
	{
        public int MaDonHang { get; set; }
        public decimal TongTien { get; set; }
        public string PhuongThuc { get; set; } // Ví dụ: "Thẻ tín dụng", "PayPal", "COD"
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoaiGiaoHang { get; set; }
    }

}
