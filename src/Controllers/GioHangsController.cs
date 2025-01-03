﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHM.Data;
using WebHM.Models;

namespace WebHM.Controllers
{
    [Authorize]
    public class GioHangsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GioHangsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống tấn công CSRF
        public async Task<IActionResult> ThemVaoGioHang(int sanPhamId, int soLuong)
        {
            if (soLuong <= 0)
            {
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy User ID từ Claims của Identity
            if (userId == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!" });
            }

            var sanPham = await _context.SanPhams.FindAsync(sanPhamId);
            if (sanPham == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            if (sanPham.SoLuongTon < soLuong)
            {
                return Json(new { success = false, message = "Số lượng sản phẩm trong kho không đủ!" });
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var gioHang = await _context.GioHangs
                .FirstOrDefaultAsync(g => g.KhachHangId == userId && g.MaSanPham == sanPhamId);

            if (gioHang != null)
            {
                // Cập nhật số lượng nếu sản phẩm đã có trong giỏ
                gioHang.SoLuong += soLuong;
                if (gioHang.SoLuong > sanPham.SoLuongTon)
                {
                    return Json(new { success = false, message = "Số lượng sản phẩm trong giỏ vượt quá tồn kho!" });
                }
                _context.Update(gioHang);
            }
            else
            {
                // Thêm sản phẩm mới vào giỏ hàng
                var newGioHang = new GioHang
                {
                    KhachHangId = userId,
                    MaSanPham = sanPhamId,
                    SoLuong = soLuong
                };
                _context.GioHangs.Add(newGioHang);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng!" });
        }



        // GET: Hiển thị giỏ hàng
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var gioHangItems = await _context.GioHangs
                .Include(g => g.SanPham)
                .Where(g => g.KhachHangId == userId)
                .ToListAsync();
            return View(gioHangItems);

        }

        // POST: Cập nhật giỏ hàng
        [HttpPost]
        public async Task<IActionResult> UpdateCart(Dictionary<int, int> soLuong)
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.GioHangs
                .Where(g => g.KhachHangId == userId)
                .ToListAsync();

            foreach (var item in cartItems)
            {
                // Kiểm tra nếu sản phẩm có trong giỏ hàng và số lượng yêu cầu trong form
                if (soLuong.ContainsKey(item.MaGioHang))
                {
                    // Lấy thông tin sản phẩm từ bảng sản phẩm để kiểm tra số lượng tồn
                    var sanPham = await _context.SanPhams
                        .FirstOrDefaultAsync(p => p.MaSanPham == item.MaSanPham);

                    if (sanPham != null)
                    {
                        // Kiểm tra số lượng yêu cầu có vượt quá số lượng tồn kho hay không
                        if (soLuong[item.MaGioHang] > sanPham.SoLuongTon)
                        {
                            // Nếu vượt quá số lượng tồn, hiển thị thông báo lỗi
                            TempData["ErrorMessage"] = $"Số lượng sản phẩm '{sanPham.TenSanPham}' không đủ trong kho.";
                            return RedirectToAction(nameof(Index)); // Quay lại trang giỏ hàng
                        }
                        else
                        {
                            // Cập nhật lại số lượng giỏ hàng
                            item.SoLuong = soLuong[item.MaGioHang];
                        }
                    }
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang giỏ hàng sau khi cập nhật
            return RedirectToAction(nameof(Index));
        }


        // POST: Xóa sản phẩm khỏi giỏ hàng
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = _userManager.GetUserId(User);
            var gioHang = await _context.GioHangs.FirstOrDefaultAsync(u=>u.KhachHangId == userId && u.MaGioHang == id);
            if (gioHang != null)
            {
                _context.GioHangs.Remove(gioHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: GioHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GioHangs == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.KhachHang)
                .Include(g => g.SanPham)
                .FirstOrDefaultAsync(m => m.MaGioHang == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // GET: GioHangs/Create
        public IActionResult Create()
        {
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: GioHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGioHang,KhachHangId,MaSanPham,SoLuong")] GioHang gioHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", gioHang.KhachHangId);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", gioHang.MaSanPham);
            return View(gioHang);
        }

        // GET: GioHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GioHangs == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHangs.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", gioHang.KhachHangId);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", gioHang.MaSanPham);
            return View(gioHang);
        }

        // POST: GioHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGioHang,KhachHangId,MaSanPham,SoLuong")] GioHang gioHang)
        {
            if (id != gioHang.MaGioHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangExists(gioHang.MaGioHang))
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
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", gioHang.KhachHangId);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", gioHang.MaSanPham);
            return View(gioHang);
        }

        // GET: GioHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GioHangs == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHangs
                .Include(g => g.KhachHang)
                .Include(g => g.SanPham)
                .FirstOrDefaultAsync(m => m.MaGioHang == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // POST: GioHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GioHangs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GioHangs'  is null.");
            }
            var gioHang = await _context.GioHangs.FindAsync(id);
            if (gioHang != null)
            {
                _context.GioHangs.Remove(gioHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangExists(int id)
        {
            return (_context.GioHangs?.Any(e => e.MaGioHang == id)).GetValueOrDefault();
        }
    }
}
