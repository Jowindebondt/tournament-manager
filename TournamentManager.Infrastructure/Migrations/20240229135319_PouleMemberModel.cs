using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PouleMemberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PouleMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PouleId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PouleMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PouleMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PouleMembers_Poules_PouleId",
                        column: x => x.PouleId,
                        principalTable: "Poules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PouleMembers_MemberId",
                table: "PouleMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PouleMembers_PouleId",
                table: "PouleMembers",
                column: "PouleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PouleMembers");
        }
    }
}
