using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class AddUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "LibraryUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "Administrator", "7d52db00-8d50-4b9b-b5ef-9948d116eed8", "Administrator", "Administrator" },
                    { "Librarian", "b41e2b52-47ca-4e13-b914-dad134bec9bd", "Bibliotekarz", "Bibliotekarz" },
                    { "Reader", "f6254c29-14c9-4e70-b19f-792ca984d378", "Czytelnik", "Czytelnik" }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Czytelnik" },
                    { 2, "Bibliotekarz" },
                    { 3, "Administrator" }
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryUsers_UserTypes_UserTypeId",
                table: "LibraryUsers");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropIndex(
                name: "IX_LibraryUsers_UserTypeId",
                table: "LibraryUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Administrator");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Librarian");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Reader");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "LibraryUsers");
        }
    }
}
