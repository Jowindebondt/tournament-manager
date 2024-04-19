using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AbstractRoundSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableTennisRoundSettings_Rounds_RoundId",
                table: "TableTennisRoundSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableTennisRoundSettings",
                table: "TableTennisRoundSettings");

            migrationBuilder.DropIndex(
                name: "IX_TableTennisRoundSettings_RoundId",
                table: "TableTennisRoundSettings");

            migrationBuilder.RenameTable(
                name: "TableTennisRoundSettings",
                newName: "RoundSettings");

            migrationBuilder.AlterColumn<int>(
                name: "BestOf",
                table: "RoundSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RoundSettings",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoundSettings",
                table: "RoundSettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoundSettings_RoundId",
                table: "RoundSettings",
                column: "RoundId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoundSettings_Rounds_RoundId",
                table: "RoundSettings",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundSettings_Rounds_RoundId",
                table: "RoundSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoundSettings",
                table: "RoundSettings");

            migrationBuilder.DropIndex(
                name: "IX_RoundSettings_RoundId",
                table: "RoundSettings");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RoundSettings");

            migrationBuilder.RenameTable(
                name: "RoundSettings",
                newName: "TableTennisRoundSettings");

            migrationBuilder.AlterColumn<int>(
                name: "BestOf",
                table: "TableTennisRoundSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableTennisRoundSettings",
                table: "TableTennisRoundSettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TableTennisRoundSettings_RoundId",
                table: "TableTennisRoundSettings",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableTennisRoundSettings_Rounds_RoundId",
                table: "TableTennisRoundSettings",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
