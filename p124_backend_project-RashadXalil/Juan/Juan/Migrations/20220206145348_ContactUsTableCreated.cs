using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class ContactUsTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactUses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(maxLength: 40, nullable: false),
                    Email = table.Column<string>(maxLength: 80, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 40, nullable: false),
                    Subject = table.Column<string>(maxLength: 20, nullable: true),
                    Message = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactUses");
        }
    }
}
