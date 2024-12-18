using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebHM.Data;
using WebHM.Models;
using WebHM.Models.Momo;
using WebHM.Services;

namespace WebHM.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        private readonly ApplicationDbContext _context;
        public PaymentController(IMomoService momoService,  ApplicationDbContext context)
        {
            _momoService = momoService;
            _context = context;

        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model, string DiaChi, string SoDienThoai, string PhuongThuc)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var gioHang = await _context.GioHangs.Include(s => s.SanPham).Where(k => k.KhachHangId == userid).ToListAsync();
            ViewBag.TongTien = gioHang.Sum(s => s.SanPham.Gia * s.SoLuong);
            /*            var orderId = $"#DH_{DateTime.Now:yyyyMMddHHmmss}";*/
            // Lưu thông tin đơn hàng vào cơ sở dữ liệu
            var donHang = new DonHang
            {
                KhachHangId = userid,
                NguoiNhan = model.FullName,
                DiaChiGiaoHang = DiaChi,
                SoDienThoaiGiaoHang = SoDienThoai,
                NgayDatHang = DateTime.Now,
                OrderId = model.OrderId,
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
/*            return RedirectToAction("XacNhanThanhToan", "ThanhToans");*/
            var response = await _momoService.CreatePaymentMomo(model);
            return Redirect(response.PayUrl);
        }
/*        [HttpGet]
        public async Task<IActionResult> PaymentCallBack(PaymentCallback paymentCallback)
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;
            if (requestQuery["resultCode"] != "0") // Giao dịch không thành công 
            {
                var donHang = _context.DonHangs.FirstOrDefault(s => s.OrderId == response.OrderId);
                if(donHang != null)
                {
                    donHang.TrangThai = "Đã thanh toán";
                }
                   await _context.SaveChangesAsync();
                return RedirectToAction("XacNhanThanhToan", "ThanhToans" , new {madonhang = requestQuery["orderId"] });
            }
            return View(response);
        }
*/
    }
}
