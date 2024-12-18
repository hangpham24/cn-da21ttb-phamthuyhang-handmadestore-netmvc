using Microsoft.AspNetCore.Mvc;
using WebHM.Data;
using WebHM.Models.Momo;
using WebHM.Services;

namespace WebHM.Controllers
{
    public class CheckoutController : Controller
    {
        private IMomoService _momoService;
        private readonly ApplicationDbContext _context;
        private readonly SanPhamService _sanPhamService;
        public CheckoutController(IMomoService momoService, ApplicationDbContext context, SanPhamService sanPhamService)
        {
            _momoService = momoService;
            _context = context;
            _sanPhamService = sanPhamService;
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCallBack(PaymentCallback paymentCallback)
        {
/*            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);*/
            var requestQuery = HttpContext.Request.Query;
            if (requestQuery["resultCode"] != "0") // Giao dịch không thành công 
            {
                var donHang = _context.DonHangs.FirstOrDefault(s => s.OrderId == paymentCallback.OrderId);
                if (donHang != null)
                {
                    await _sanPhamService.CapNhatSoLuongTonKho(donHang.MaDonHang);
                    donHang.TrangThai = "Đã thanh toán";
                }

                await _context.SaveChangesAsync();


                return RedirectToAction("XacNhanThanhToan", "ThanhToans", new { madonhang = donHang.MaDonHang });
            }




            // Tạo đối tượng PaymentCallback từ các tham số
            var callbackData = new PaymentCallback
            {
                PartnerCode = paymentCallback.PartnerCode,
                AccessKey = paymentCallback.AccessKey,
                RequestId = paymentCallback.RequestId,
                Amount = paymentCallback.Amount,
                OrderId = paymentCallback.OrderId,
                OrderInfo = Uri.UnescapeDataString(paymentCallback.OrderInfo),
                OrderType = paymentCallback.OrderType,
                TransId = paymentCallback.TransId,
                Message = Uri.UnescapeDataString(paymentCallback.Message),
                LocalMessage = Uri.UnescapeDataString(paymentCallback.LocalMessage),
                ResponseTime = paymentCallback.ResponseTime,
                ErrorCode = paymentCallback.ErrorCode,
                PayType = paymentCallback.PayType,
                ExtraData = paymentCallback.ExtraData,
                Signature = paymentCallback.Signature
            };

            // Trả về view với dữ liệu callback
            return View(callbackData);
        }
        /*public IActionResult PaymentCallBack(string partnerCode, string accessKey, string requestId,
                                             string amount, string orderId, string orderInfo,
                                             string orderType, string transId, string message,
                                             string localMessage, string responseTime, string errorCode,
                                             string payType, string extraData, string signature)
        {
            // Tạo đối tượng PaymentCallback từ các tham số
            var callbackData = new PaymentCallback
            {
                PartnerCode = partnerCode,
                AccessKey = accessKey,
                RequestId = requestId,
                Amount = amount,
                OrderId = orderId,
                OrderInfo = Uri.UnescapeDataString(orderInfo),
                OrderType = orderType,
                TransId = transId,
                Message = Uri.UnescapeDataString(message),
                LocalMessage = Uri.UnescapeDataString(localMessage),
                ResponseTime = responseTime,
                ErrorCode = errorCode,
                PayType = payType,
                ExtraData = extraData,
                Signature = signature
            };

            // Trả về view với dữ liệu callback
            return View(callbackData);
        }*/
    }
}
