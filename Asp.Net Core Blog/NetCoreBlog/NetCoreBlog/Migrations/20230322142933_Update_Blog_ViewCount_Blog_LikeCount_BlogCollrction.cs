using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCoreBlog.Migrations
{
    public partial class Update_Blog_ViewCount_Blog_LikeCount_BlogCollrction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiveLikeCount",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BlogCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BlogInfoDtoId = table.Column<int>(type: "int", nullable: false),
                    CustomerIdentityUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogCollection_AspNetUsers_CustomerIdentityUserId",
                        column: x => x.CustomerIdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogCollection_Blog_BlogInfoDtoId",
                        column: x => x.BlogInfoDtoId,
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCollection_BlogInfoDtoId",
                table: "BlogCollection",
                column: "BlogInfoDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCollection_CustomerIdentityUserId",
                table: "BlogCollection",
                column: "CustomerIdentityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogCollection");

            migrationBuilder.DropColumn(
                name: "GiveLikeCount",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Blog");
        }
    }
}
