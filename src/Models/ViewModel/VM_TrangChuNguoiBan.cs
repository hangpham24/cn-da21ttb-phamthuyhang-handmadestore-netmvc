namespace WebHM.Models.ViewModel
{
    public class VM_TrangChuNguoiBan
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; } // Tổng số đơn hàng
        public decimal Revenue { get; set; }
        public List<ChiTietDonHang> RecentOrders { get; set; } // Đơn hàng gần đây

        public List<DonHang> DonHangChoXuLy { get; set; } // Đơn hàng chờ xử lý
    }
}
