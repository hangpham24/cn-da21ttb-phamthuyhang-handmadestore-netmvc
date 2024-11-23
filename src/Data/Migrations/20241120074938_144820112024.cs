using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHM.Data.Migrations
{
    public partial class _144820112024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToans_AspNetUsers_KhachHangId",
                table: "ThanhToans");

            migrationBuilder.DropIndex(
                name: "IX_ThanhToans_KhachHangId",
                table: "ThanhToans");

            migrationBuilder.DropColumn(
                name: "KhachHangId",
                table: "ThanhToans");

            migrationBuilder.AddColumn<int>(
                name: "MaDonHang",
                table: "ThanhToans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaDonHang",
                table: "ThanhToans",
                column: "MaDonHang");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToans_DonHangs_MaDonHang",
                table: "ThanhToans",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToans_DonHangs_MaDonHang",
                table: "ThanhToans");

            migrationBuilder.DropIndex(
                name: "IX_ThanhToans_MaDonHang",
                table: "ThanhToans");

            migrationBuilder.DropColumn(
                name: "MaDonHang",
                table: "ThanhToans");

            migrationBuilder.AddColumn<string>(
                name: "KhachHangId",
                table: "ThanhToans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_KhachHangId",
                table: "ThanhToans",
                column: "KhachHangId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToans_AspNetUsers_KhachHangId",
                table: "ThanhToans",
                column: "KhachHangId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
