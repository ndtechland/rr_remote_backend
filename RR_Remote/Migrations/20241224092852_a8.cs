using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR_Remote.Migrations
{
    public partial class a8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "CategoryMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "CategoryMasters");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Brands");
        }
    }
}
