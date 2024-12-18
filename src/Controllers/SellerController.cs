using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebHM.Data;
using WebHM.Services; // Service hoặc lớp xử lý
using System.Linq;
using WebHM.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebHM.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Tổng sản phẩm
            var totalProducts = await _context.SanPhams.CountAsync(sp => sp.NguoiBanId == sellerId);

            // Tổng đơn hàng
            var totalOrders = await _context.DonHangs
                .Where(dh => dh.ChiTietDonHangs.Any(ct => ct.SanPham.NguoiBanId == sellerId))
                .CountAsync();

            // Doanh thu từ các đơn hàng có trạng thái là "Đã giao" hoặc "Đã thanh toán"
            var revenue = await _context.ChiTietDonHangs
                .Where(ct => ct.SanPham.NguoiBanId == sellerId &&
                             (ct.DonHang.TrangThai == "Đã giao" || ct.DonHang.TrangThai == "Đã thanh toán"))
                .SumAsync(ct => ct.SoLuong * ct.Gia);

            // Đơn hàng gần đây (10 đơn hàng mới nhất)
            var recentOrders = await _context.ChiTietDonHangs
                .Where(ct => ct.SanPham.NguoiBanId == sellerId)
                .OrderByDescending(ct => ct.DonHang.NgayDatHang)
                .Take(10)
                .ToListAsync();

            // Đơn hàng cần xử lý
            var donHangChoXuLy = await _context.DonHangs
                .Where(dh => dh.TrangThai == "Chờ xử lý" &&
                             dh.ChiTietDonHangs.Any(ct => ct.SanPham.NguoiBanId == sellerId))
                .Include(dh => dh.ChiTietDonHangs)
                .ToListAsync();

            // Doanh thu theo tháng
            var doanhThuTheoThang = await _context.ChiTietDonHangs
                .Join(_context.SanPhams, ct => ct.MaSanPham, sp => sp.MaSanPham, (ct, sp) => new { ct, sp })
                .Join(_context.DonHangs, cs => cs.ct.MaDonHang, dh => dh.MaDonHang, (cs, dh) => new { cs.ct, cs.sp, dh })
                .Where(c => c.sp.NguoiBanId == sellerId && c.dh.TrangThai == "Đã thanh toán" || c.dh.TrangThai == "Đã giao")
                .GroupBy(c => new { c.dh.NgayDatHang.Month, c.dh.NgayDatHang.Year })
                .Select(g => new
                {
                    Thang = g.Key.Month,
                    Nam = g.Key.Year,
                    DoanhThu = g.Sum(e => e.ct.SoLuong * e.ct.Gia)
                })
                .OrderBy(g => g.Nam).ThenBy(g => g.Thang)
                .ToListAsync();

            // Sản phẩm bán chạy nhất (Top 5)
            var bestSellingProducts = await _context.ChiTietDonHangs
                .Join(_context.SanPhams, ct => ct.MaSanPham, sp => sp.MaSanPham, (ct, sp) => new { ct, sp })
                .Join(_context.DonHangs, cs => cs.ct.MaDonHang, dh => dh.MaDonHang, (cs, dh) => new { cs.ct, cs.sp, dh })
                .Where(x => x.sp.NguoiBanId == sellerId && x.dh.TrangThai == "Đã thanh toán")
                .GroupBy(x => new { x.sp.TenSanPham, x.sp.HinhAnh })
                .Select(g => new VM_SanPhamBanChay
                {
                    TenSanPham = g.Key.TenSanPham,
                    TongSoLuong = g.Sum(x => x.ct.SoLuong),
                    HinhAnh = g.Key.HinhAnh
                })
                .OrderByDescending(x => x.TongSoLuong)
                .Take(5)
                .ToListAsync();

            // Chuẩn bị dữ liệu biểu đồ
            var labels = doanhThuTheoThang.Select(x => $"{x.Thang}/{x.Nam}").ToList();
            var data = doanhThuTheoThang.Select(x => x.DoanhThu).ToList();

            // ViewModel tổng hợp
            var viewModel = new VM_TrangChuNguoiBan
            {
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                Revenue = revenue,
                RecentOrders = recentOrders,
                DonHangChoXuLy = donHangChoXuLy
            };

            ViewBag.Labels = labels;
            ViewBag.Data = data;
            ViewBag.BestSellingProducts = bestSellingProducts;

            return View(viewModel);
        }
    }
}
