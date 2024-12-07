using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHM.Data;
using WebHM.Models;

namespace WebHM.Controllers
{
    public class ChiTietDonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDonHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietDonHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChiTietDonHangs.Include(c => c.DonHang).Include(c => c.SanPham);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChiTietDonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.DonHang)
                .Include(c => c.SanPham)
                .FirstOrDefaultAsync(m => m.MaChiTiet == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Create
        public IActionResult Create()
        {
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang");
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: ChiTietDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaChiTiet,MaDonHang,MaSanPham,SoLuong,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietDonHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietDonHang.MaSanPham);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietDonHang.MaSanPham);
            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaChiTiet,MaDonHang,MaSanPham,SoLuong,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.MaChiTiet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDonHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDonHangExists(chiTietDonHang.MaChiTiet))
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
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietDonHang.MaSanPham);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.DonHang)
                .Include(c => c.SanPham)
                .FirstOrDefaultAsync(m => m.MaChiTiet == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChiTietDonHangs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ChiTietDonHangs'  is null.");
            }
            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang != null)
            {
                _context.ChiTietDonHangs.Remove(chiTietDonHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietDonHangExists(int id)
        {
          return (_context.ChiTietDonHangs?.Any(e => e.MaChiTiet == id)).GetValueOrDefault();
        }
    }
}
