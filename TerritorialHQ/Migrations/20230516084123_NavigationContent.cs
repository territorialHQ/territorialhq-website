using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerritorialHQ.Migrations
{
    /// <inheritdoc />
    public partial class NavigationContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentPages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserCreated = table.Column<string>(type: "longtext", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: true),
                    SidebarContent = table.Column<string>(type: "longtext", nullable: true),
                    BannerImage = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPages", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NavigationEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Slug = table.Column<string>(type: "longtext", nullable: false),
                    Public = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentId = table.Column<string>(type: "varchar(255)", nullable: true),
                    Order = table.Column<short>(type: "smallint", nullable: false),
                    ContentPageId = table.Column<string>(type: "varchar(255)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavigationEntries_ContentPages_ContentPageId",
                        column: x => x.ContentPageId,
                        principalTable: "ContentPages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NavigationEntries_NavigationEntries_ParentId",
                        column: x => x.ParentId,
                        principalTable: "NavigationEntries",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NavigationEntries_ContentPageId",
                table: "NavigationEntries",
                column: "ContentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_NavigationEntries_ParentId",
                table: "NavigationEntries",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavigationEntries");

            migrationBuilder.DropTable(
                name: "ContentPages");
        }
    }
}
