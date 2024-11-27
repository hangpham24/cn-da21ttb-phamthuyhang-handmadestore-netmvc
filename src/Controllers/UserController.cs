using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebHM.Models;
using WebHM.Services;

namespace WebHM.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ Admin mới có quyền truy cập
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Danh sách người dùng
        public async Task<IActionResult> Index(string query, int pageIndex = 1)
        {
            IQueryable<ApplicationUser> users = _userManager.Users;

            // Xử lý tìm kiếm
            if (!string.IsNullOrWhiteSpace(query))
            {
                users = users.Where(u => u.UserName.Contains(query) || u.Email.Contains(query));
                ViewBag.SearchQuery = query; // Lưu giá trị để hiển thị lại trên ô tìm kiếm
            }

            int pageSize = 5; // Số người dùng trên mỗi trang
            var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsync(users, pageIndex, pageSize);

            return View(paginatedUsers);
        }



        // Thêm người dùng mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser model, IFormFile HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file ảnh upload
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(HinhAnh.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png).");
                        return View(model);
                    }

                    if (HinhAnh.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                    {
                        ModelState.AddModelError("", "Kích thước file không được vượt quá 5MB.");
                        return View(model);
                    }

                    try
                    {
                        // Định nghĩa đường dẫn thư mục upload
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        // Định nghĩa tên file
                        var fileName = Guid.NewGuid() + extension;
                        var filePath = Path.Combine(uploadDir, fileName);

                        // Lưu file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn ảnh vào model
                        model.UrlAnhDaiDien = "/images/avatars/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu ảnh: " + ex.Message);
                        return View(model);
                    }
                }

                // Tạo người dùng mới
                var newUser = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    NgaySinh = model.NgaySinh,
                    UrlAnhDaiDien = model.UrlAnhDaiDien,
                    TenCuaHang = model.TenCuaHang,
                    DiaChiCuaHang = model.DiaChiCuaHang,
                    MaSoThue = model.MaSoThue,
                    NgayDangKyCuaHang = model.NgayDangKyCuaHang
                };

                var result = await _userManager.CreateAsync(newUser, "Default@123"); // Tạo user với mật khẩu mặc định

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Tạo người dùng mới thành công!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo người dùng.";
            return View(model);
        }

        // Sửa thông tin người dùng
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model, IFormFile HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra người dùng có tồn tại không
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                // Xử lý file ảnh upload
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(HinhAnh.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png).");
                        return View(model);
                    }

                    if (HinhAnh.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                    {
                        ModelState.AddModelError("", "Kích thước file không được vượt quá 5MB.");
                        return View(model);
                    }

                    try
                    {
                        // Định nghĩa đường dẫn thư mục upload
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        // Định nghĩa tên file
                        var fileName = Guid.NewGuid() + extension;
                        var filePath = Path.Combine(uploadDir, fileName);

                        // Lưu file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn ảnh vào model
                        model.UrlAnhDaiDien = "/images/avatars/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu ảnh: " + ex.Message);
                        return View(model);
                    }
                }

                // Cập nhật thông tin người dùng
                user.HoTen = model.HoTen;
                user.DiaChi = model.DiaChi;
                user.NgaySinh = model.NgaySinh;
                user.UrlAnhDaiDien = model.UrlAnhDaiDien ?? user.UrlAnhDaiDien;
                user.TenCuaHang = model.TenCuaHang;
                user.DiaChiCuaHang = model.DiaChiCuaHang;
                user.MaSoThue = model.MaSoThue;
                user.NgayDangKyCuaHang = model.NgayDangKyCuaHang;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Cập nhật thông tin người dùng thành công!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật người dùng.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Xóa người dùng thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa người dùng.";
            return RedirectToAction(nameof(Index));
        }

    }
}
