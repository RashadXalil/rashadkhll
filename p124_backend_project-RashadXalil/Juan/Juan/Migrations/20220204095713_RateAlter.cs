using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class RateAlter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Reviews",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Rate",
                table: "Reviews",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
