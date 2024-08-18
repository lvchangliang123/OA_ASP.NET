using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueNetBlog.Server.Migrations
{
    public partial class UpdateBlogModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodePath",
                schema: "VueNetBlogDB_Test",
                table: "Blog",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverPath",
                schema: "VueNetBlogDB_Test",
                table: "Blog",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodePath",
                schema: "VueNetBlogDB_Test",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "CoverPath",
                schema: "VueNetBlogDB_Test",
                table: "Blog");
        }
    }
}
