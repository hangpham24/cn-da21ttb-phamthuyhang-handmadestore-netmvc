using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHM.Data;
using WebHM.Models;
using WebHM.Services;

namespace WebHM.Controllers
{
    public class ThanhToansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SanPhamService _sanPhamService;
        public ThanhToansController(ApplicationDbContext context, SanPhamService sanPhamService)
        {
            _context = context;
            _sanPhamService = sanPhamService;
        }
        [HttpPost]
        public async Task<IActionResult> ThanhToan(string FullName, string DiaChi, string SoDienThoai, string PhuongThuc, string OrderId)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var gioHang = await _context.GioHangs.Include(s => s.SanPham).Where(k => k.KhachHangId == userid).ToListAsync();
            ViewBag.TongTien = gioHang.Sum(s => s.SanPham.Gia * s.SoLuong);
/*            var orderId = $"#DH_{DateTime.Now:yyyyMMddHHmmss}";*/
            // Lưu thông tin đơn hàng vào cơ sở dữ liệu
            var donHang = new DonHang
            {
                KhachHangId = userid,
                NguoiNhan = FullName,
                DiaChiGiaoHang = DiaChi,
                SoDienThoaiGiaoHang = SoDienThoai,
                NgayDatHang = DateTime.Now,
                OrderId = @DateTime.UtcNow.Ticks.ToString(),
                TongTien = Convert.ToDecimal(ViewBag.TongTien) + 25000, // Tổng tiền bao gồm phí vận chuyển
                TrangThai = "Chờ xử lý"
            };

            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();
            var gioHangs = await _context.GioHangs.Include(s => s.SanPham).Where(k => k.KhachHangId == userid).ToListAsync();
            ViewBag.ListSanPham = gioHangs;
            // Lưu chi tiết đơn hàng
            foreach (var item in ViewBag.ListSanPham)
            {
                var chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSanPham = item.SanPham.MaSanPham,
                    SoLuong = item.SoLuong,
                    Gia = item.SanPham.Gia
                };

                _context.ChiTietDonHangs.Add(chiTietDonHang);
            }

            await _context.SaveChangesAsync();

            var donHangCapNhat = await _context.DonHangs.FirstOrDefaultAsync(d => d.OrderId == OrderId);
           if (donHangCapNhat != null)
            {
                await _sanPhamService.CapNhatSoLuongTonKho(donHangCapNhat.MaDonHang);
            }

            // Lưu thông tin thanh toán
            var thanhToan = new ThanhToan
            {
                MaDonHang = donHang.MaDonHang,
                SoTien = donHang.TongTien,
                NgayThanhToan = DateTime.Now,
                PhuongThuc = PhuongThuc
            };

            _context.ThanhToans.Add(thanhToan);
            await _context.SaveChangesAsync();

/*            if (PhuongThuc == "Momo")
            {
                // Redirect to Momo payment page
                return RedirectToAction("CreatePaymentMomo", "Payment");
            }*/

            // Sau khi lưu dữ liệu, chuyển hướng đến trang hoàn tất
            return RedirectToAction("XacNhanThanhToan", "ThanhToans");
        }

        [HttpGet]
        public async Task<IActionResult> XacNhanThanhToan(int maDonHang)
        {
            // Lấy thông tin người dùng
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Tìm đơn hàng theo mã đơn hàng và thuộc về người dùng hiện tại
            var donHang = await _context.DonHangs
                .Where(dh => dh.KhachHangId == userid && dh.MaDonHang == maDonHang)
                .FirstOrDefaultAsync();

            if (donHang == null)
            {
                // Nếu không tìm thấy đơn hàng
                return RedirectToAction("Index", "Home");
            }

            // Lấy thông tin chi tiết đơn hàng (sản phẩm)
            var chiTietDonHangs = await _context.ChiTietDonHangs
                .Include(ct => ct.SanPham)
                .Where(ct => ct.MaDonHang == donHang.MaDonHang)
                .ToListAsync();

            // Tính tổng giá trị đơn hàng (bao gồm phí vận chuyển)
            var tongTien = donHang.TongTien;
            var phiVanChuyen = 25000; // Giả sử phí vận chuyển là 25,000 VND
            var tongTienFinal = tongTien;/*+ phiVanChuyen;*/

            // Truyền dữ liệu vào ViewBag hoặc ViewData để hiển thị trên view
            ViewBag.DonHang = donHang;
            ViewBag.ChiTietDonHangs = chiTietDonHangs;
            ViewBag.TongTienFinal = tongTienFinal;
            ViewBag.PhiVanChuyen = phiVanChuyen;

            return View();
        }

        // GET: ThanhToans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ThanhToans.Include(t => t.DonHang);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ThanhToans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ThanhToans == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.DonHang)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // GET: ThanhToans/Create
        public IActionResult Create()
        {
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang");
            return View();
        }

        // POST: ThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThanhToan,MaDonHang,SoTien,NgayThanhToan,PhuongThuc")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhToan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", thanhToan.MaDonHang);
            return View(thanhToan);
        }

        // GET: ThanhToans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ThanhToans == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan == null)
            {
                return NotFound();
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", thanhToan.MaDonHang);
            return View(thanhToan);
        }

        // POST: ThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThanhToan,MaDonHang,SoTien,NgayThanhToan,PhuongThuc")] ThanhToan thanhToan)
        {
            if (id != thanhToan.MaThanhToan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.MaThanhToan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", thanhToan.MaDonHang);
            return View(thanhToan);
        }

        // GET: ThanhToans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ThanhToans == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.DonHang)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // POST: ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ThanhToans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ThanhToans'  is null.");
            }
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToans.Remove(thanhToan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhToanExists(int id)
        {
          return (_context.ThanhToans?.Any(e => e.MaThanhToan == id)).GetValueOrDefault();
        }
    }
}
