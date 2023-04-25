using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCoreBlog.Migrations
{
    public partial class Update_BlogInfo_Summary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogSummary",
                table: "Blog",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogSummary",
                table: "Blog");
        }
    }
}
