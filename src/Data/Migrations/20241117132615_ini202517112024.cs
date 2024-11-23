using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHM.Data.Migrations
{
    public partial class ini202517112024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiemDanhGia",
                table: "BinhLuans",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiemDanhGia",
                table: "BinhLuans");
        }
    }
}
