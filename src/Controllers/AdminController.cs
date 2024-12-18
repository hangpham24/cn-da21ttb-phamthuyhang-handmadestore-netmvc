using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHM.Data;

namespace WebHM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Tổng số sản phẩm
            int totalProducts = await _context.SanPhams.CountAsync();

            // Tổng số đơn hàng
            int totalOrders = await _context.DonHangs.CountAsync();

            // Tổng doanh thu
            decimal totalRevenue = await _context.DonHangs
                .Where(dh => dh.TrangThai == "Đã giao")
                .SumAsync(dh => dh.TongTien);

            // Số khách hàng mới (giả định là trong tháng này)
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int newCustomers = await _context.Users
                .Where(u => u.NgayTaoTaiKhoan >= startOfMonth)
                .CountAsync();

            // Đơn hàng đang chờ xử lý
            var pendingOrders = await _context.DonHangs
                .Where(dh => dh.TrangThai == "Đang xử lý")
                .OrderByDescending(dh => dh.NgayDatHang)
                .Take(5) // Lấy tối đa 5 đơn hàng
                .ToListAsync();

            // Sản phẩm bán chạy nhất (giả định theo số lượng trong ChiTietDonHang)
            var bestSellingProducts = await _context.ChiTietDonHangs
                .GroupBy(ct => new { ct.MaSanPham, ct.SanPham.TenSanPham })
                .Select(g => new
                {
                    ProductName = g.Key.TenSanPham,
                    TotalSold = g.Sum(ct => ct.SoLuong)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(5) // Lấy tối đa 5 sản phẩm bán chạy
                .ToListAsync();

            // Truyền dữ liệu qua ViewBag
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.NewCustomers = newCustomers;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.BestSellingProducts = bestSellingProducts;

            return View();
        }
    }
}
