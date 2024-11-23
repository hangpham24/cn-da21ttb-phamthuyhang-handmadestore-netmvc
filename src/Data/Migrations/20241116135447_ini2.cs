using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHM.Data.Migrations
{
    public partial class ini2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChiGiaoHang",
                table: "DonHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoaiGiaoHang",
                table: "DonHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "VanChuyens",
                columns: table => new
                {
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaVanChuyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhiVanChuyen = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanChuyens", x => x.MaVanChuyen);
                });

            migrationBuilder.CreateTable(
                name: "DonHangVanChuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangVanChuyens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonHangVanChuyens_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangVanChuyens_VanChuyens_MaVanChuyen",
                        column: x => x.MaVanChuyen,
                        principalTable: "VanChuyens",
                        principalColumn: "MaVanChuyen",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHangVanChuyens_MaDonHang",
                table: "DonHangVanChuyens",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangVanChuyens_MaVanChuyen",
                table: "DonHangVanChuyens",
                column: "MaVanChuyen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonHangVanChuyens");

            migrationBuilder.DropTable(
                name: "VanChuyens");

            migrationBuilder.DropColumn(
                name: "DiaChiGiaoHang",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "SoDienThoaiGiaoHang",
                table: "DonHangs");
        }
    }
}
