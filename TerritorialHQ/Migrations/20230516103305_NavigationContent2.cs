using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerritorialHQ.Migrations
{
    /// <inheritdoc />
    public partial class NavigationContent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ContentPages");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "ContentPages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ContentPages",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCreated",
                table: "ContentPages",
                type: "longtext",
                nullable: true);
        }
    }
}
