using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCoreBlog.Migrations
{
    public partial class UpdateUserAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentUserAvatar",
                table: "BlogComment",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Blog",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentUserAvatar",
                table: "BlogComment");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Blog");
        }
    }
}
