using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class HexCodeColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HexCode",
                table: "Colors",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HexCode",
                table: "Colors");
        }
    }
}
