using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class ChangedModelNamesAndSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingCarts_Users_UserId",
                table: "BorrowingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOutRecords_Librarians_LibrarianId",
                table: "CheckOutRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRecords_Librarians_LibrarianId",
                table: "ReturnRecords");

            migrationBuilder.DropTable(
                name: "ContactDetails");

            migrationBuilder.DropTable(
                name: "ContactDetailTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Librarians",
                table: "Librarians");

            migrationBuilder.RenameTable(
                name: "Librarians",
                newName: "LibraryUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Loans",
                newName: "LibraryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                newName: "IX_Loans_LibraryUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BorrowingCarts",
                newName: "LibraryUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowingCarts_UserId",
                table: "BorrowingCarts",
                newName: "IX_BorrowingCarts_LibraryUserId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LibraryUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LibraryUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "LibraryUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "LibraryUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LibraryUsers",
                table: "LibraryUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_LibraryUsers_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "LibraryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingCarts_LibraryUsers_LibraryUserId",
                table: "BorrowingCarts",
                column: "LibraryUserId",
                principalTable: "LibraryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOutRecords_LibraryUsers_LibrarianId",
                table: "CheckOutRecords",
                column: "LibrarianId",
                principalTable: "LibraryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LibraryUsers_LibraryUserId",
                table: "Loans",
                column: "LibraryUserId",
                principalTable: "LibraryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRecords_LibraryUsers_LibrarianId",
                table: "ReturnRecords",
                column: "LibrarianId",
                principalTable: "LibraryUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_LibraryUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowingCarts_LibraryUsers_LibraryUserId",
                table: "BorrowingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOutRecords_LibraryUsers_LibrarianId",
                table: "CheckOutRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LibraryUsers_LibraryUserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRecords_LibraryUsers_LibrarianId",
                table: "ReturnRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LibraryUsers",
                table: "LibraryUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LibraryUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "LibraryUsers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "LibraryUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "LibraryUsers");

            migrationBuilder.RenameTable(
                name: "LibraryUsers",
                newName: "Librarians");

            migrationBuilder.RenameColumn(
                name: "LibraryUserId",
                table: "Loans",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_LibraryUserId",
                table: "Loans",
                newName: "IX_Loans_UserId");

            migrationBuilder.RenameColumn(
                name: "LibraryUserId",
                table: "BorrowingCarts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowingCarts_LibraryUserId",
                table: "BorrowingCarts",
                newName: "IX_BorrowingCarts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Librarians",
                table: "Librarians",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ContactDetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetailTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContactDetailInformation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactDetails_ContactDetailTypes_ContactDetailTypeId",
                        column: x => x.ContactDetailTypeId,
                        principalTable: "ContactDetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_ContactDetailTypeId",
                table: "ContactDetails",
                column: "ContactDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_UserId",
                table: "ContactDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowingCarts_Users_UserId",
                table: "BorrowingCarts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOutRecords_Librarians_LibrarianId",
                table: "CheckOutRecords",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRecords_Librarians_LibrarianId",
                table: "ReturnRecords",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id");
        }
    }
}
