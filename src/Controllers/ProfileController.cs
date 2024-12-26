using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebHM.Models;

namespace WebHM.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UpdateProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            return View(user); // Truyền model vào view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ApplicationUser model, IFormFile UrlAnhDaiDien)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng hiện tại
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            // Kiểm tra role để xác định logic xử lý
            bool isSeller = await _userManager.IsInRoleAsync(user, "Seller");

            if (UrlAnhDaiDien != null)
            {
                // Xử lý ảnh đại diện
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UrlAnhDaiDien.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UrlAnhDaiDien.CopyToAsync(stream);
                }

                // Xóa ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(user.UrlAnhDaiDien))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.UrlAnhDaiDien.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                user.UrlAnhDaiDien = "/images/avatars/" + fileName;
            }

            if (isSeller)
            {
                // Cập nhật thông tin người bán
                user.TenCuaHang = model.TenCuaHang;
                user.DiaChiCuaHang = model.DiaChiCuaHang;
                user.MaSoThue = model.MaSoThue;
            }
            else
            {
                // Cập nhật thông tin người dùng (khách hàng)
                user.HoTen = model.HoTen;
                user.DiaChi = model.DiaChi;
                user.NgaySinh = model.NgaySinh;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("UpdateProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

    }
}
