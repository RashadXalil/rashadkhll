using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class RedirectUrlColumnDropFromSliders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "Sliders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "Sliders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
