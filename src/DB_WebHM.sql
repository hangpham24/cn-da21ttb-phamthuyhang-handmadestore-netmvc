USE [WebHM]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[DiaChi] [nvarchar](max) NULL,
	[DiaChiCuaHang] [nvarchar](max) NULL,
	[HoTen] [nvarchar](max) NULL,
	[MaSoThue] [nvarchar](max) NULL,
	[NgayDangKyCuaHang] [datetime2](7) NULL,
	[NgaySinh] [datetime2](7) NULL,
	[NgayTaoTaiKhoan] [datetime2](7) NULL,
	[TenCuaHang] [nvarchar](max) NULL,
	[UrlAnhDaiDien] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BinhLuans]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BinhLuans](
	[MaBinhLuan] [int] IDENTITY(1,1) NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[KhachHangId] [nvarchar](450) NOT NULL,
	[NoiDungBinhLuan] [nvarchar](max) NOT NULL,
	[NgayBinhLuan] [datetime2](7) NOT NULL,
	[DiemDanhGia] [int] NULL,
 CONSTRAINT [PK_BinhLuans] PRIMARY KEY CLUSTERED 
(
	[MaBinhLuan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietDonHangs]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHangs](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ChiTietDonHangs] PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucs]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucs](
	[MaDanhMuc] [int] IDENTITY(1,1) NOT NULL,
	[TenDanhMuc] [nvarchar](max) NOT NULL,
	[MoTa] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DanhMucs] PRIMARY KEY CLUSTERED 
(
	[MaDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHangs]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHangs](
	[MaDonHang] [int] IDENTITY(1,1) NOT NULL,
	[KhachHangId] [nvarchar](450) NOT NULL,
	[NgayDatHang] [datetime2](7) NOT NULL,
	[TongTien] [decimal](18, 2) NOT NULL,
	[TrangThai] [nvarchar](max) NOT NULL,
	[DiaChiGiaoHang] [nvarchar](max) NOT NULL,
	[SoDienThoaiGiaoHang] [nvarchar](max) NOT NULL,
	[NguoiNhan] [nvarchar](max) NOT NULL,
	[OrderId] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DonHangs] PRIMARY KEY CLUSTERED 
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHangVanChuyens]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHangVanChuyens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[MaVanChuyen] [int] NOT NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
	[NgayCapNhat] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DonHangVanChuyens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GioHangs]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GioHangs](
	[MaGioHang] [int] IDENTITY(1,1) NOT NULL,
	[KhachHangId] [nvarchar](450) NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
 CONSTRAINT [PK_GioHangs] PRIMARY KEY CLUSTERED 
(
	[MaGioHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichSuThanhToans]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichSuThanhToans](
	[MaLichSu] [int] IDENTITY(1,1) NOT NULL,
	[MaThanhToan] [int] NOT NULL,
	[NgayGiaoDich] [datetime2](7) NOT NULL,
	[SoTien] [decimal](18, 2) NOT NULL,
	[TrangThai] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LichSuThanhToans] PRIMARY KEY CLUSTERED 
(
	[MaLichSu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPhams]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPhams](
	[MaSanPham] [int] IDENTITY(1,1) NOT NULL,
	[TenSanPham] [nvarchar](255) NOT NULL,
	[Gia] [decimal](18, 2) NOT NULL,
	[MoTa] [nvarchar](max) NOT NULL,
	[HinhAnh] [nvarchar](max) NOT NULL,
	[MaDanhMuc] [int] NOT NULL,
	[NguoiBanId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_SanPhams] PRIMARY KEY CLUSTERED 
(
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThanhToans]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThanhToans](
	[MaThanhToan] [int] IDENTITY(1,1) NOT NULL,
	[SoTien] [decimal](18, 2) NOT NULL,
	[NgayThanhToan] [datetime2](7) NOT NULL,
	[PhuongThuc] [nvarchar](max) NOT NULL,
	[MaDonHang] [int] NOT NULL,
 CONSTRAINT [PK_ThanhToans] PRIMARY KEY CLUSTERED 
(
	[MaThanhToan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThongKeDoanhSos]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThongKeDoanhSos](
	[NguoiBanId] [nvarchar](450) NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[NgayBan] [datetime2](7) NOT NULL,
	[SoLuongDaBan] [int] NOT NULL,
	[TongDoanhThu] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ThongKeDoanhSos] PRIMARY KEY CLUSTERED 
(
	[NguoiBanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VanChuyens]    Script Date: 11/23/2024 9:08:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VanChuyens](
	[MaVanChuyen] [int] IDENTITY(1,1) NOT NULL,
	[TenNhaVanChuyen] [nvarchar](100) NOT NULL,
	[PhiVanChuyen] [decimal](18, 2) NOT NULL,
	[MoTa] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_VanChuyens] PRIMARY KEY CLUSTERED 
(
	[MaVanChuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[DonHangs] ADD  DEFAULT (N'') FOR [DiaChiGiaoHang]
GO
ALTER TABLE [dbo].[DonHangs] ADD  DEFAULT (N'') FOR [SoDienThoaiGiaoHang]
GO
ALTER TABLE [dbo].[DonHangs] ADD  DEFAULT (N'') FOR [NguoiNhan]
GO
ALTER TABLE [dbo].[DonHangs] ADD  DEFAULT (N'') FOR [OrderId]
GO
ALTER TABLE [dbo].[ThanhToans] ADD  DEFAULT ((0)) FOR [MaDonHang]
GO
ALTER TABLE [dbo].[VanChuyens] ADD  DEFAULT ((0.0)) FOR [PhiVanChuyen]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[BinhLuans]  WITH CHECK ADD  CONSTRAINT [FK_BinhLuans_AspNetUsers_KhachHangId] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[BinhLuans] CHECK CONSTRAINT [FK_BinhLuans_AspNetUsers_KhachHangId]
GO
ALTER TABLE [dbo].[BinhLuans]  WITH CHECK ADD  CONSTRAINT [FK_BinhLuans_SanPhams_MaSanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPhams] ([MaSanPham])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BinhLuans] CHECK CONSTRAINT [FK_BinhLuans_SanPhams_MaSanPham]
GO
ALTER TABLE [dbo].[ChiTietDonHangs]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHangs_DonHangs_MaDonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHangs] ([MaDonHang])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHangs] CHECK CONSTRAINT [FK_ChiTietDonHangs_DonHangs_MaDonHang]
GO
ALTER TABLE [dbo].[ChiTietDonHangs]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHangs_SanPhams_MaSanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPhams] ([MaSanPham])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHangs] CHECK CONSTRAINT [FK_ChiTietDonHangs_SanPhams_MaSanPham]
GO
ALTER TABLE [dbo].[DonHangs]  WITH CHECK ADD  CONSTRAINT [FK_DonHangs_AspNetUsers_KhachHangId] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[DonHangs] CHECK CONSTRAINT [FK_DonHangs_AspNetUsers_KhachHangId]
GO
ALTER TABLE [dbo].[DonHangVanChuyens]  WITH CHECK ADD  CONSTRAINT [FK_DonHangVanChuyens_DonHangs_MaDonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHangs] ([MaDonHang])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DonHangVanChuyens] CHECK CONSTRAINT [FK_DonHangVanChuyens_DonHangs_MaDonHang]
GO
ALTER TABLE [dbo].[DonHangVanChuyens]  WITH CHECK ADD  CONSTRAINT [FK_DonHangVanChuyens_VanChuyens_MaVanChuyen] FOREIGN KEY([MaVanChuyen])
REFERENCES [dbo].[VanChuyens] ([MaVanChuyen])
GO
ALTER TABLE [dbo].[DonHangVanChuyens] CHECK CONSTRAINT [FK_DonHangVanChuyens_VanChuyens_MaVanChuyen]
GO
ALTER TABLE [dbo].[GioHangs]  WITH CHECK ADD  CONSTRAINT [FK_GioHangs_AspNetUsers_KhachHangId] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[GioHangs] CHECK CONSTRAINT [FK_GioHangs_AspNetUsers_KhachHangId]
GO
ALTER TABLE [dbo].[GioHangs]  WITH CHECK ADD  CONSTRAINT [FK_GioHangs_SanPhams_MaSanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPhams] ([MaSanPham])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GioHangs] CHECK CONSTRAINT [FK_GioHangs_SanPhams_MaSanPham]
GO
ALTER TABLE [dbo].[LichSuThanhToans]  WITH CHECK ADD  CONSTRAINT [FK_LichSuThanhToans_ThanhToans_MaThanhToan] FOREIGN KEY([MaThanhToan])
REFERENCES [dbo].[ThanhToans] ([MaThanhToan])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LichSuThanhToans] CHECK CONSTRAINT [FK_LichSuThanhToans_ThanhToans_MaThanhToan]
GO
ALTER TABLE [dbo].[SanPhams]  WITH CHECK ADD  CONSTRAINT [FK_SanPhams_AspNetUsers_NguoiBanId] FOREIGN KEY([NguoiBanId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[SanPhams] CHECK CONSTRAINT [FK_SanPhams_AspNetUsers_NguoiBanId]
GO
ALTER TABLE [dbo].[SanPhams]  WITH CHECK ADD  CONSTRAINT [FK_SanPhams_DanhMucs_MaDanhMuc] FOREIGN KEY([MaDanhMuc])
REFERENCES [dbo].[DanhMucs] ([MaDanhMuc])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SanPhams] CHECK CONSTRAINT [FK_SanPhams_DanhMucs_MaDanhMuc]
GO
ALTER TABLE [dbo].[ThanhToans]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToans_DonHangs_MaDonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHangs] ([MaDonHang])
GO
ALTER TABLE [dbo].[ThanhToans] CHECK CONSTRAINT [FK_ThanhToans_DonHangs_MaDonHang]
GO
ALTER TABLE [dbo].[ThongKeDoanhSos]  WITH CHECK ADD  CONSTRAINT [FK_ThongKeDoanhSos_AspNetUsers_NguoiBanId] FOREIGN KEY([NguoiBanId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ThongKeDoanhSos] CHECK CONSTRAINT [FK_ThongKeDoanhSos_AspNetUsers_NguoiBanId]
GO
ALTER TABLE [dbo].[ThongKeDoanhSos]  WITH CHECK ADD  CONSTRAINT [FK_ThongKeDoanhSos_SanPhams_MaSanPham] FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPhams] ([MaSanPham])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThongKeDoanhSos] CHECK CONSTRAINT [FK_ThongKeDoanhSos_SanPhams_MaSanPham]
GO
