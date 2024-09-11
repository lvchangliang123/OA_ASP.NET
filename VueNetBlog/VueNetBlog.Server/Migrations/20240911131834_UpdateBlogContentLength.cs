using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueNetBlog.Server.Migrations
{
    public partial class UpdateBlogContentLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "VueNetBlogDB_Test",
                table: "Blog",
                type: "longtext",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "VueNetBlogDB_Test",
                table: "Blog",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldMaxLength: 2147483647,
                oldNullable: true);
        }
    }
}
