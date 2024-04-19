using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExplicitFkUsagesMatchModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player_1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player_2Id",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "Player_2Id",
                table: "Matches",
                newName: "Player2Id");

            migrationBuilder.RenameColumn(
                name: "Player_1Id",
                table: "Matches",
                newName: "Player1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Player_2Id",
                table: "Matches",
                newName: "IX_Matches_Player2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Player_1Id",
                table: "Matches",
                newName: "IX_Matches_Player1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player1Id",
                table: "Matches",
                column: "Player1Id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player2Id",
                table: "Matches",
                column: "Player2Id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player2Id",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "Player2Id",
                table: "Matches",
                newName: "Player_2Id");

            migrationBuilder.RenameColumn(
                name: "Player1Id",
                table: "Matches",
                newName: "Player_1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Player2Id",
                table: "Matches",
                newName: "IX_Matches_Player_2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Player1Id",
                table: "Matches",
                newName: "IX_Matches_Player_1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player_1Id",
                table: "Matches",
                column: "Player_1Id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Players_Player_2Id",
                table: "Matches",
                column: "Player_2Id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
