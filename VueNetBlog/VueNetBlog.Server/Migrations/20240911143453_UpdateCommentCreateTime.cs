using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueNetBlog.Server.Migrations
{
    public partial class UpdateCommentCreateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                schema: "VueNetBlogDB_Test",
                table: "Comment",
                type: "datetime",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                schema: "VueNetBlogDB_Test",
                table: "Comment");
        }
    }
}
