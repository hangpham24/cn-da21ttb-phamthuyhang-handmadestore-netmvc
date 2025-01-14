﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHM.Data;
using WebHM.Models;
using WebHM.Services;

namespace WebHM.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult HuyDonHang([FromForm] int maDonHang)
		{
			if (maDonHang <= 0)
			{
				return BadRequest("Mã đơn hàng không hợp lệ.");
			}

			var donHang = _context.DonHangs.FirstOrDefault(d => d.MaDonHang == maDonHang);
			if (donHang == null || donHang.TrangThai != "Chờ xử lý")
			{
				return NotFound("Không tìm thấy đơn hàng hoặc trạng thái không hợp lệ.");
			}

			donHang.TrangThai = "Hủy";
			_context.Update(donHang);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> ThongKeDoanhSo(string nguoiBanId, int? thang, int? nam, int? danhMucId, int pageIndex = 1)
        {
            if(User.IsInRole("Seller"))
            {
                nguoiBanId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            var query = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.SanPham)
                        .ThenInclude(s => s.DanhMuc)  // Bao gồm danh mục sản phẩm
                .Include(d => d.KhachHang)
                .Where(d => d.KhachHangId != null); // Không bao gồm đơn hàng không có khách hàng

            // Lọc theo người bán
            if (!string.IsNullOrEmpty(nguoiBanId))
            {
                query = query.Where(d => d.ChiTietDonHangs.Any(c => c.SanPham.NguoiBanId == nguoiBanId));
            }

            // Lọc theo tháng và năm
            if (thang.HasValue && thang.Value != 0)
            {
                query = query.Where(d => d.NgayDatHang.Month == thang);
            }
            else if (thang == 0) // Tháng này
            {
                query = query.Where(d => d.NgayDatHang.Month == DateTime.Now.Month);
            }

            if (nam.HasValue && nam.Value != 0)
            {
                query = query.Where(d => d.NgayDatHang.Year == nam);
            }
            else if (nam == 0) // Năm nay
            {
                query = query.Where(d => d.NgayDatHang.Year == DateTime.Now.Year);
            }

            // Lọc theo danh mục sản phẩm
            if (danhMucId.HasValue && danhMucId.Value != 0)
            {
                query = query.Where(d => d.ChiTietDonHangs.Any(c => c.SanPham.MaDanhMuc == danhMucId));
            }

            // Tạo danh sách phân trang
            var pageSize = 5; // Số lượng kết quả mỗi trang
            var paginatedList = await PaginatedList<dynamic>.CreateAsync(
                query.Select(donHang => new
                {
                    DonHang = donHang,
                    SoLuongDonHang = donHang.ChiTietDonHangs.Sum(c => c.SoLuong),
                    TongTien = donHang.ChiTietDonHangs.Sum(c => c.SoLuong * c.Gia),
                    SoLuongHuy = donHang.TrangThai == "Hủy" ? donHang.ChiTietDonHangs.Sum(c => c.SoLuong) : 0,
                    SanPhamTen = donHang.ChiTietDonHangs.Select(c => c.SanPham.TenSanPham).ToList(),
                    DanhMucTen = donHang.ChiTietDonHangs.Select(c => c.SanPham.DanhMuc.TenDanhMuc).FirstOrDefault() // Lấy tên danh mục của sản phẩm
                }), pageIndex, pageSize
            );

            // Truyền danh sách người bán vào ViewData để hiển thị
            ViewData["NguoiBans"] = await _context.Users.ToListAsync();
            ViewData["DanhMucs"] = await _context.DanhMucs.ToListAsync(); // Lấy danh sách danh mục sản phẩm

            // Truyền phân trang vào View
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = paginatedList.TotalPages;
            ViewBag.HasPreviousPage = paginatedList.HasPreviousPage;
            ViewBag.HasNextPage = paginatedList.HasNextPage;

            return View(paginatedList);
        }


        // POST: DonHangs/CapNhatTrangThai
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> CapNhatTrangThai(int maDonHang)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy userId của người dùng hiện tại

            // Kiểm tra nếu người dùng có vai trò "Seller"
            if (User.IsInRole("Seller") || User.IsInRole("Admin"))
            {
                // Tìm đơn hàng cần cập nhật
                var donHang = await _context.DonHangs.Include(d => d.KhachHang)
                                                      .Include(d => d.ChiTietDonHangs)
                                                      .ThenInclude(ctd => ctd.SanPham)
                                                      .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);

                if (donHang == null)
                {
                    return NotFound(); // Nếu không tìm thấy đơn hàng
                }

                // Kiểm tra nếu người bán có quyền thay đổi trạng thái của đơn hàng này
                bool isSellerOfProduct = donHang.ChiTietDonHangs.Any(ctd => ctd.SanPham.NguoiBanId == userId);
                if (!isSellerOfProduct)
                {
                    return Forbid(); // Nếu người bán không phải là người bán sản phẩm trong đơn hàng này
                }

                if(donHang.TrangThai == "Đã thanh toán" || donHang.TrangThai == "Đã xác nhận")
                {
                    donHang.TrangThai = "Đã giao";
                } else
                {
                    donHang.TrangThai = "Đã xác nhận";
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Update(donHang);
                await _context.SaveChangesAsync();

                // Trả về thông báo thành công hoặc quay lại trang danh sách đơn hàng
                return RedirectToAction(nameof(Index));
            }

            return Unauthorized(); // Trả về Unauthorized nếu người dùng không phải seller
        }


        // Action để hiển thị danh sách đơn hàng
        public async Task<IActionResult> DanhSachDonHang()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID người dùng hiện tại

            // Lấy tất cả đơn hàng của người dùng từ cơ sở dữ liệu
            var donHangs = await _context.DonHangs
                                        .Where(d => d.KhachHangId == userid)
                                        .Include(d => d.ChiTietDonHangs)  // Bao gồm thông tin chi tiết đơn hàng
                                        .ThenInclude(cd => cd.SanPham)   // Bao gồm sản phẩm trong chi tiết đơn hàng
                                        .OrderByDescending(d => d.NgayDatHang)  // Sắp xếp theo ngày đặt hàng (mới nhất lên trên)
                                        .ToListAsync();

            return View(donHangs);  // Truyền danh sách đơn hàng vào View
        }

        // Action để hiển thị chi tiết đơn hàng
        public async Task<IActionResult> XacNhanThanhToan(int id)
        {
            var donHang = await _context.DonHangs
                                        .Include(d => d.ChiTietDonHangs)
                                        .ThenInclude(cd => cd.SanPham)
                                        .FirstOrDefaultAsync(d => d.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);  // Truyền đơn hàng vào view để hiển thị chi tiết
        }

        public async Task<IActionResult> ThanhToan()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gioHangs = await _context.GioHangs.Include(s => s.SanPham).Where(k => k.KhachHangId == userId).ToListAsync();
            ViewBag.ListSanPham = gioHangs;
            ViewBag.TongTien = gioHangs.Sum(s => s.SanPham.Gia * s.SoLuong);
            return View(gioHangs);
        }


        // GET: DonHangs
        [Authorize]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5, string searchString = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy User ID của người đang đăng nhập
            var applicationDbContext = _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ctd => ctd.SanPham);

            IQueryable<DonHang> query;

            if (User.IsInRole("Seller"))
            {
                query = applicationDbContext
                    .Where(d => d.ChiTietDonHangs.Any(ctd => ctd.SanPham.NguoiBanId == userId));
            }
            else if (User.IsInRole("Customer"))
            {
                query = applicationDbContext
                    .Where(d => d.KhachHangId == userId);
            }
            else
            {
                query = applicationDbContext;
            }

            // Xử lý tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => d.OrderId.Contains(searchString) ||
                                         d.KhachHang.HoTen.Contains(searchString) ||
                                         d.TrangThai.Contains(searchString));
            }

            query = query.OrderByDescending(d => d.TrangThai == "Chờ xử lý") // Sắp xếp ưu tiên "Chờ xử lý"
                         .ThenByDescending(d => d.NgayDatHang);

            // Tạo danh sách phân trang
            var paginatedList = await PaginatedList<DonHang>.CreateAsync(query, pageIndex, pageSize);

            return View(paginatedList);
        }




        // GET: DonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DonHangs == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHangs/Create
        public IActionResult Create()
        {
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDonHang,KhachHangId,NguoiNhan,DiaChiGiaoHang,SoDienThoaiGiaoHang,NgayDatHang,TongTien,TrangThai,OrderId")] DonHang donHang)
        {

                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", donHang.KhachHangId);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DonHangs == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", donHang.KhachHangId);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDonHang,KhachHangId,NguoiNhan,DiaChiGiaoHang,SoDienThoaiGiaoHang,NgayDatHang,TongTien,TrangThai,OrderId")] DonHang donHang)
        {
            if (id != donHang.MaDonHang)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDonHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
           
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", donHang.KhachHangId);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DonHangs == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DonHangs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DonHangs'  is null.");
            }
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
          return (_context.DonHangs?.Any(e => e.MaDonHang == id)).GetValueOrDefault();
        }
    }
}
