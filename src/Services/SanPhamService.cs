using Microsoft.EntityFrameworkCore;
using WebHM.Data;

namespace WebHM.Services
{
    public class SanPhamService
    {
        private readonly ApplicationDbContext _context;

        public SanPhamService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CapNhatSoLuongTonKho(int maDonHang)
        {
            // Lấy chi tiết đơn hàng
            var chitietdonhang = await _context.ChiTietDonHangs.Where(tk => tk.MaDonHang == maDonHang).ToListAsync();

            // Duyệt qua từng sản phẩm trong đơn hàng để trừ số lượng tồn kho
            foreach (var chiTiet in chitietdonhang)
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSanPham);

                if (sanPham != null)
                {
                    // Trừ số lượng tồn kho
                    sanPham.SoLuongTon -= chiTiet.SoLuong;

                    // Kiểm tra tồn kho không được nhỏ hơn 0
                    if (sanPham.SoLuongTon < 0)
                    {
                        sanPham.SoLuongTon = 0;
                    }
                }
            }

            // Lưu lại thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
    }
}
