using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalBack.Migrations
{
    public partial class BlogChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Blogs",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Blogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Blogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
