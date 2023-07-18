using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class ChangedRelationBetweenLibraryUserAndUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryUsers_UserTypes_UserTypeId",
                table: "LibraryUsers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryUsers_UserTypeId",
                table: "LibraryUsers");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "LibraryUsers");

            migrationBuilder.CreateTable(
                name: "LibraryUserUserType",
                columns: table => new
                {
                    LibraryUsersId = table.Column<int>(type: "int", nullable: false),
                    UserTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryUserUserType", x => new { x.LibraryUsersId, x.UserTypesId });
                    table.ForeignKey(
                        name: "FK_LibraryUserUserType_LibraryUsers_LibraryUsersId",
                        column: x => x.LibraryUsersId,
                        principalTable: "LibraryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryUserUserType_UserTypes_UserTypesId",
                        column: x => x.UserTypesId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Administrator",
                column: "ConcurrencyStamp",
                value: "25a2607d-1e67-443e-bb65-0749d1e074f0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Librarian",
                column: "ConcurrencyStamp",
                value: "bf71615a-a80b-454d-88f4-83441759d8cc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Reader",
                column: "ConcurrencyStamp",
                value: "00f8712d-8656-46ca-aee7-81aed10ad64a");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryUserUserType_UserTypesId",
                table: "LibraryUserUserType",
                column: "UserTypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryUserUserType");

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "LibraryUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Administrator",
                column: "ConcurrencyStamp",
                value: "7d52db00-8d50-4b9b-b5ef-9948d116eed8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Librarian",
                column: "ConcurrencyStamp",
                value: "b41e2b52-47ca-4e13-b914-dad134bec9bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Reader",
                column: "ConcurrencyStamp",
                value: "f6254c29-14c9-4e70-b19f-792ca984d378");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryUsers_UserTypeId",
                table: "LibraryUsers",
                column: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryUsers_UserTypes_UserTypeId",
                table: "LibraryUsers",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "Id");
        }
    }
}
