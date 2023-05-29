using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerritorialHQ.Migrations
{
    /// <inheritdoc />
    public partial class ClanReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InReview",
                table: "Clans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Clans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InReview",
                table: "Clans");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Clans");
        }
    }
}
