using System;
using System.Collections.Generic;
using System.IO;
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
using WebHM.Services;

namespace WebHM.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SanPhamsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SanPhams

        public async Task<IActionResult> Index(string query, int pageIndex = 1)
        {
            IQueryable<SanPham> sanPhams = _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.NguoiBan);

            // Lấy danh sách danh mục
            var listDanhMuc = await _context.DanhMucs.ToListAsync();
            ViewBag.ListDanhMuc = listDanhMuc;

            // Kiểm tra vai trò
            if (User.IsInRole("Admin"))
            {
                // Admin xem toàn bộ sản phẩm
            }
            else if (User.IsInRole("Seller"))
            {
                // Seller chỉ xem sản phẩm của mình
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sanPhams = sanPhams.Where(s => s.NguoiBanId == userId);
            }

            // Xử lý tìm kiếm
            if (!string.IsNullOrWhiteSpace(query))
            {
                sanPhams = sanPhams.Where(s => s.TenSanPham.Contains(query));
                ViewBag.SearchQuery = query; // Lưu để hiển thị lại trong ô tìm kiếm
            }

            int pageSize = 3; // Số sản phẩm trên mỗi trang
                              // Lấy danh sách sản phẩm phân trang
            var phantrangSanPhams = await PaginatedList<SanPham>.CreateAsync(sanPhams, pageIndex, pageSize);

            return View(phantrangSanPhams);
        }



        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }
            var product = _context.SanPhams
        .Include(p => p.DanhMuc)
        .Include(p => p.NguoiBan)
        .FirstOrDefault(p => p.MaSanPham == id);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy các sản phẩm liên quan (cùng danh mục, trừ sản phẩm hiện tại)
            var relatedProducts = _context.SanPhams
                .Where(p => p.MaDanhMuc == product.MaDanhMuc && p.MaSanPham != id)
                .Take(3) // Giới hạn 3 sản phẩm liên quan
                .ToList();


            var sanPham = await _context.SanPhams
      .Include(s => s.DanhMuc)
      .Include(s => s.NguoiBan)
      .Include(p => p.BinhLuans)
          .ThenInclude(bl => bl.KhachHang) // Include thông tin người bình luận
      .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }
            // Truyền sản phẩm hiện tại và danh sách sản phẩm liên quan vào view
            ViewBag.RelatedProducts = relatedProducts;
            return View(sanPham);
        }

        // POST: SanPhams/AddComment/id
        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string Content, int Rating)
        {
            var product = _context.SanPhams.Include(p => p.BinhLuans).ThenInclude(bl => bl.KhachHang).FirstOrDefault(p => p.MaSanPham == id);

            if (product != null)
            {
                // Lấy ID người dùng hiện tại
                var userId = _userManager.GetUserId(User); // Lấy User ID từ ASP.NET Identity

                var comment = new BinhLuan
                {
                    MaSanPham = id,
                    NoiDungBinhLuan = Content,
                    DiemDanhGia = Rating,
                    KhachHangId = userId, // Lưu User ID vào bình luận
                    NgayBinhLuan = DateTime.Now
                };

                _context.BinhLuans.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id)
        {
            // Kiểm tra xem bình luận có tồn tại không
            var comment = await _context.BinhLuans.Include(c => c.KhachHang).FirstOrDefaultAsync(c => c.MaBinhLuan == id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.KhachHang.UserName == User.Identity.Name )
                {
                // Xóa bình luận
                _context.BinhLuans.Remove(comment);
            }
            else
            {
                return Forbid();
            }
              await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = comment.MaSanPham });
        }



        // GET: SanPhams/Create
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Create()
        {
            if (User.IsInRole("Seller"))
            {
                ViewData["NguoiBanId"] = new SelectList(new[] { User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }
            else
            {
                ViewData["NguoiBanId"] = new SelectList(_context.Users, "Id", "UserName");
            }

            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");

            return View();
        }

        // POST: SanPhams/Create
        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,Gia,MoTa,HinhAnh,MaDanhMuc,NguoiBanId")] SanPham sanPham, IFormFile HinhAnh)
        {
            if (HinhAnh != null && HinhAnh.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sanphams", Guid.NewGuid().ToString() + Path.GetExtension(HinhAnh.FileName));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await HinhAnh.CopyToAsync(stream);
                }

                sanPham.HinhAnh = "/images/sanphams/" + Path.GetFileName(filePath);
            }

            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(currentUserId);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Seller"))
                {
                    sanPham.NguoiBanId = currentUserId;
                }
            }

            _context.Add(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SanPhams/Edit/5
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            // Lọc người dùng có role "Seller"
            var sellerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Seller");
            var sellers = await _context.UserRoles
                .Where(ur => ur.RoleId == sellerRole.Id) // Lấy UserRole có RoleId là "Seller"
                .Join(_context.Users, ur => ur.UserId, u => u.Id, (ur, u) => u) // Lấy danh sách người dùng
                .ToListAsync();

            // Tạo SelectList chỉ chứa những người dùng có role "Seller"
            ViewData["NguoiBanId"] = new SelectList(sellers, "Id", "TenCuaHang", sanPham.NguoiBanId);

            // Lấy danh sách Danh Mục
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);

            return View(sanPham);
        }


        // POST: SanPhams/Edit/5
        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSanPham,TenSanPham,Gia,MoTa,HinhAnh,MaDanhMuc,NguoiBanId")] SanPham sanPham, IFormFile newImage)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }

            // Tải sản phẩm từ cơ sở dữ liệu
            var existingProduct = await _context.SanPhams.AsNoTracking().FirstOrDefaultAsync(sp => sp.MaSanPham == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu có ảnh mới được tải lên
            if (newImage != null && newImage.Length > 0)
            {
                // Xóa ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(existingProduct.HinhAnh))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingProduct.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath); // Xóa file ảnh
                            Console.WriteLine("Đã xóa ảnh: " + imagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Lỗi khi xóa ảnh: " + ex.Message);
                        }
                    }
                }

                // Tạo tên file mới với GUID để đảm bảo duy nhất
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
                var filePath = Path.Combine("wwwroot", "images", "sanphams", fileName);

                // Lưu ảnh mới vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn ảnh vào đối tượng sản phẩm
                sanPham.HinhAnh = "/images/sanphams/" + fileName;
            }
            else
            {
                // Giữ nguyên ảnh cũ
                sanPham.HinhAnh = existingProduct.HinhAnh;
            }

            try
            {
                // Cập nhật sản phẩm trong cơ sở dữ liệu
                _context.Update(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanPham.MaSanPham))
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



        // GET: SanPhams/Delete/5
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.NguoiBan)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [Authorize(Roles = "Admin,Seller")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SanPhams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SanPhams' is null.");
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                // Xóa ảnh trong thư mục nếu tồn tại
                if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", sanPham.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath); // Xóa file ảnh
                            Console.WriteLine("Đã xóa ảnh: " + imagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Lỗi khi xóa ảnh: " + ex.Message);
                        }
                    }
                }

                // Xóa sản phẩm khỏi cơ sở dữ liệu
                _context.SanPhams.Remove(sanPham);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool SanPhamExists(int id)
        {
            return (_context.SanPhams?.Any(e => e.MaSanPham == id)).GetValueOrDefault();
        }
    }
}
