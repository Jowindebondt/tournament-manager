using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetAbstractModelSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentSettings_TournamentId",
                table: "TournamentSettings");

            migrationBuilder.DropIndex(
                name: "IX_RoundSettings_RoundId",
                table: "RoundSettings");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "TournamentSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "RoundSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSettings_TournamentId",
                table: "TournamentSettings",
                column: "TournamentId",
                unique: true,
                filter: "[TournamentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoundSettings_RoundId",
                table: "RoundSettings",
                column: "RoundId",
                unique: true,
                filter: "[RoundId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentSettings_TournamentId",
                table: "TournamentSettings");

            migrationBuilder.DropIndex(
                name: "IX_RoundSettings_RoundId",
                table: "RoundSettings");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "TournamentSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoundId",
                table: "RoundSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSettings_TournamentId",
                table: "TournamentSettings",
                column: "TournamentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoundSettings_RoundId",
                table: "RoundSettings",
                column: "RoundId",
                unique: true);
        }
    }
}
