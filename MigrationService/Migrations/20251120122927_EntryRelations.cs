using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationService.Migrations
{
    /// <inheritdoc />
    public partial class EntryRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Groups_GroupId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_GroupId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "People");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Entries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "GroupPerson",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPerson", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupPerson_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPerson_People_UsersId",
                        column: x => x.UsersId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_GroupId",
                table: "Entries",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPerson_UsersId",
                table: "GroupPerson",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Groups_GroupId",
                table: "Entries",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Groups_GroupId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "GroupPerson");

            migrationBuilder.DropIndex(
                name: "IX_Entries_GroupId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Entries");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "People",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_GroupId",
                table: "People",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Groups_GroupId",
                table: "People",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
