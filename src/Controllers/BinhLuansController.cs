using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHM.Data;
using WebHM.Models;
using WebHM.Services;

namespace WebHM.Controllers
{
    public class BinhLuansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BinhLuansController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaBinhLuan(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bình luận." });
            }

            var binhLuan = await _context.BinhLuans.FindAsync(id);

            if (binhLuan == null)
            {
                return Json(new { success = false, message = "Bình luận không tồn tại." });
            }

            _context.BinhLuans.Remove(binhLuan);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa bình luận thành công." });
        }


        // GET: BinhLuans
        public async Task<IActionResult> Index(string searchString, int pageIndex = 1)
        {
            // Lọc bình luận theo từ khóa tìm kiếm
            var applicationDbContext = _context.BinhLuans
                .Include(b => b.KhachHang)
                .Include(b => b.SanPham)  // Đảm bảo rằng .Include được gọi đúng kiểu dữ liệu
                .AsQueryable();  // Đảm bảo rằng kết quả là một IQueryable

            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(b => b.KhachHang.HoTen.Contains(searchString) || b.NoiDungBinhLuan.Contains(searchString));
            }

            // Định nghĩa số lượng bản ghi mỗi trang
            int pageSize = 5;
            var count = await applicationDbContext.CountAsync(); // Đếm tổng số bản ghi
            var items = await applicationDbContext
                                .Skip((pageIndex - 1) * pageSize) 
                                .Take(pageSize)                    
                                .ToListAsync();

            var paginatedList = new PaginatedList<BinhLuan>(items, count, pageIndex, pageSize);

            // Trả lại view cùng dữ liệu phân trang và tìm kiếm
            ViewBag.SearchQuery = searchString; // Để giữ giá trị tìm kiếm trong input
            return View(paginatedList);
        }


        // GET: BinhLuans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BinhLuans == null)
            {
                return NotFound();
            }

            var binhLuan = await _context.BinhLuans
                .Include(b => b.KhachHang)
                .Include(b => b.SanPham)
                .FirstOrDefaultAsync(m => m.MaBinhLuan == id);
            if (binhLuan == null)
            {
                return NotFound();
            }

            return View(binhLuan);
        }

        // GET: BinhLuans/Create
        public IActionResult Create()
        {
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: BinhLuans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBinhLuan,MaSanPham,KhachHangId,DiemDanhGia,NoiDungBinhLuan,NgayBinhLuan")] BinhLuan binhLuan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(binhLuan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", binhLuan.KhachHangId);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", binhLuan.MaSanPham);
            return View(binhLuan);
        }

        // GET: BinhLuans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BinhLuans == null)
            {
                return NotFound();
            }

            var binhLuan = await _context.BinhLuans.FindAsync(id);
            if (binhLuan == null)
            {
                return NotFound();
            }
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", binhLuan.KhachHangId);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", binhLuan.MaSanPham);
            return View(binhLuan);
        }

        // POST: BinhLuans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaBinhLuan,MaSanPham,KhachHangId,DiemDanhGia,NoiDungBinhLuan,NgayBinhLuan")] BinhLuan binhLuan)
        {
            if (id != binhLuan.MaBinhLuan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(binhLuan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BinhLuanExists(binhLuan.MaBinhLuan))
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
            ViewData["KhachHangId"] = new SelectList(_context.Users, "Id", "Id", binhLuan.KhachHangId);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", binhLuan.MaSanPham);
            return View(binhLuan);
        }

        // GET: BinhLuans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BinhLuans == null)
            {
                return NotFound();
            }

            var binhLuan = await _context.BinhLuans
                .Include(b => b.KhachHang)
                .Include(b => b.SanPham)
                .FirstOrDefaultAsync(m => m.MaBinhLuan == id);
            if (binhLuan == null)
            {
                return NotFound();
            }

            return View(binhLuan);
        }

        // POST: BinhLuans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BinhLuans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BinhLuans'  is null.");
            }
            var binhLuan = await _context.BinhLuans.FindAsync(id);
            if (binhLuan != null)
            {
                _context.BinhLuans.Remove(binhLuan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BinhLuanExists(int id)
        {
          return (_context.BinhLuans?.Any(e => e.MaBinhLuan == id)).GetValueOrDefault();
        }
    }
}
