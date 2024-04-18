using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Members_Player_1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Members_Player_2Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_PouleMembers_Members_MemberId",
                table: "PouleMembers");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "PouleMembers",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PouleMembers_MemberId",
                table: "PouleMembers",
                newName: "IX_PouleMembers_PlayerId");

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_PlayerId",
                table: "Members",
                column: "PlayerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Players_PlayerId",
                table: "Members",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PouleMembers_Players_PlayerId",
                table: "PouleMembers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player_1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Players_Player_2Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Players_PlayerId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_PouleMembers_Players_PlayerId",
                table: "PouleMembers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Members_PlayerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "PouleMembers",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_PouleMembers_PlayerId",
                table: "PouleMembers",
                newName: "IX_PouleMembers_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Members_Player_1Id",
                table: "Matches",
                column: "Player_1Id",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Members_Player_2Id",
                table: "Matches",
                column: "Player_2Id",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PouleMembers_Members_MemberId",
                table: "PouleMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
