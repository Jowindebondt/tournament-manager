using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AbstractTournamentSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableTennisSettings_Tournaments_TournamentId",
                table: "TableTennisSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableTennisSettings",
                table: "TableTennisSettings");

            migrationBuilder.DropIndex(
                name: "IX_TableTennisSettings_TournamentId",
                table: "TableTennisSettings");

            migrationBuilder.RenameTable(
                name: "TableTennisSettings",
                newName: "TournamentSettings");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentType",
                table: "TournamentSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Handicap",
                table: "TournamentSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "TournamentSettings",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentSettings",
                table: "TournamentSettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSettings_TournamentId",
                table: "TournamentSettings",
                column: "TournamentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSettings_Tournaments_TournamentId",
                table: "TournamentSettings",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSettings_Tournaments_TournamentId",
                table: "TournamentSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentSettings",
                table: "TournamentSettings");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSettings_TournamentId",
                table: "TournamentSettings");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "TournamentSettings");

            migrationBuilder.RenameTable(
                name: "TournamentSettings",
                newName: "TableTennisSettings");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentType",
                table: "TableTennisSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Handicap",
                table: "TableTennisSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableTennisSettings",
                table: "TableTennisSettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TableTennisSettings_TournamentId",
                table: "TableTennisSettings",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableTennisSettings_Tournaments_TournamentId",
                table: "TableTennisSettings",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
