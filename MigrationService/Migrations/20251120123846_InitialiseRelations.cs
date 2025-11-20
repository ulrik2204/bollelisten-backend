using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialiseRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupPerson_People_UsersId",
                table: "GroupPerson");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "GroupPerson",
                newName: "PeopleId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupPerson_UsersId",
                table: "GroupPerson",
                newName: "IX_GroupPerson_PeopleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPerson_People_PeopleId",
                table: "GroupPerson",
                column: "PeopleId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupPerson_People_PeopleId",
                table: "GroupPerson");

            migrationBuilder.RenameColumn(
                name: "PeopleId",
                table: "GroupPerson",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupPerson_PeopleId",
                table: "GroupPerson",
                newName: "IX_GroupPerson_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPerson_People_UsersId",
                table: "GroupPerson",
                column: "UsersId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
