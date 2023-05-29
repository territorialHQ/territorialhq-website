using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerritorialHQ.Migrations
{
    /// <inheritdoc />
    public partial class Journal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JournalArticles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Subtitle = table.Column<string>(type: "longtext", nullable: true),
                    PublishFrom = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PublishTo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Teaser = table.Column<string>(type: "longtext", nullable: true),
                    Body = table.Column<string>(type: "longtext", nullable: true),
                    Image = table.Column<string>(type: "longtext", nullable: true),
                    Tags = table.Column<string>(type: "longtext", nullable: true),
                    IsSticky = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalArticles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalArticles");
        }
    }
}
