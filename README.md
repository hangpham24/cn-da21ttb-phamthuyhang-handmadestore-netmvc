# WebHM - Đồ Án Chuyên Ngành

## Giới thiệu
WebHM là một ứng dụng web được phát triển bằng ASP.NET Core, được thực hiện như một phần của đồ án chuyên ngành.

## Chức năng chính
- **Quản lý sản phẩm**: Thêm, sửa, xóa và hiển thị sản phẩm
- **Quản lý danh mục**: Phân loại và tổ chức sản phẩm theo danh mục
- **Giỏ hàng**: Thêm sản phẩm vào giỏ hàng, cập nhật số lượng
- **Đặt hàng và thanh toán**: Xử lý đơn hàng và thanh toán trực tuyến
- **Quản lý đơn hàng**: Theo dõi và cập nhật trạng thái đơn hàng
- **Bình luận và đánh giá**: Người dùng có thể bình luận và đánh giá sản phẩm
- **Hệ thống người dùng**: Đăng ký, đăng nhập, quản lý thông tin cá nhân
- **Quản lý người bán (Seller)**: Giao diện riêng cho người bán quản lý sản phẩm
- **Trang Admin**: Quản lý toàn bộ hệ thống, người dùng và đơn hàng

## Công nghệ sử dụng
- ASP.NET Core
- Entity Framework Core
- SQL Server
- HTML/CSS/JavaScript
- Bootstrap

## Cấu trúc dự án
- `Areas`: Chứa các module của ứng dụng
- `Components`: Chứa các view component
- `Controllers`: Chứa các controller xử lý logic
- `Models`: Chứa các model và entity
- `Views`: Chứa các file giao diện
- `Services`: Chứa các service xử lý business logic
- `wwwroot`: Chứa các file tĩnh (CSS, JS, Images)

## Cài đặt và Chạy
1. Yêu cầu hệ thống:
   - .NET Core SDK
   - SQL Server
   - Visual Studio (khuyến nghị)

2. Các bước cài đặt:
   - Clone repository này
   - Restore database từ file `DB_WebHM.sql`
   - Cập nhật connection string trong `appsettings.json`
   - Build và chạy ứng dụng

## Thông tin liên hệ
- Họ và tên: [Phạm Thúy Hằng]
- Email: [hangnguyenpham2424@gmail.com]
- Số điện thoại: [0393150005]
- MSSV: [110121182]
