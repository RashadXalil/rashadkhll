using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AlterFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "Features");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Features",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Features",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Features");

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "Features",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
