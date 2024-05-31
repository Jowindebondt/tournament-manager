using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePouleMemberForManyToManyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PouleMembers");

            migrationBuilder.CreateTable(
                name: "PlayerPoule",
                columns: table => new
                {
                    PlayersId = table.Column<int>(type: "int", nullable: false),
                    PoulesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPoule", x => new { x.PlayersId, x.PoulesId });
                    table.ForeignKey(
                        name: "FK_PlayerPoule_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerPoule_Poules_PoulesId",
                        column: x => x.PoulesId,
                        principalTable: "Poules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPoule_PoulesId",
                table: "PlayerPoule",
                column: "PoulesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerPoule");

            migrationBuilder.CreateTable(
                name: "PouleMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    PouleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PouleMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PouleMembers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
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
                name: "IX_PouleMembers_PlayerId",
                table: "PouleMembers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PouleMembers_PouleId",
                table: "PouleMembers",
                column: "PouleId");
        }
    }
}
