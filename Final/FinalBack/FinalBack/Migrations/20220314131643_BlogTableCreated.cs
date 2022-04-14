using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalBack.Migrations
{
    public partial class BlogTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    View = table.Column<int>(maxLength: 100, nullable: false),
                    Desc1 = table.Column<string>(maxLength: 200, nullable: true),
                    MainImage = table.Column<string>(maxLength: 200, nullable: true),
                    Title1 = table.Column<string>(maxLength: 200, nullable: true),
                    Desc2 = table.Column<string>(maxLength: 200, nullable: true),
                    Title2 = table.Column<string>(maxLength: 200, nullable: true),
                    Image2 = table.Column<string>(maxLength: 200, nullable: true),
                    Desc3 = table.Column<string>(maxLength: 200, nullable: true),
                    BlogCatId = table.Column<int>(maxLength: 200, nullable: false),
                    BlogCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCategoryId",
                table: "Blogs",
                column: "BlogCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
