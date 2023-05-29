using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerritorialHQ.Migrations
{
    /// <inheritdoc />
    public partial class Clans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    LogoFile = table.Column<string>(type: "longtext", nullable: true),
                    BannerFile = table.Column<string>(type: "longtext", nullable: true),
                    DiscordLink = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clans", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClanUserRelations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClanId = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanUserRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClanUserRelations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClanUserRelations_Clans_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClanUserRelations_ClanId",
                table: "ClanUserRelations",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanUserRelations_UserId",
                table: "ClanUserRelations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanUserRelations");

            migrationBuilder.DropTable(
                name: "Clans");
        }
    }
}
