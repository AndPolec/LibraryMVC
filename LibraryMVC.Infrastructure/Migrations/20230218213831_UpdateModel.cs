using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class UpdateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Reservations_ReservationId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Librarians_CheckInLibrarianId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Librarians_CheckOutLibrarianId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Loans_CheckInLibrarianId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CheckInLibrarianId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "Loans",
                newName: "LoanCreationDate");

            migrationBuilder.RenameColumn(
                name: "CheckOutLibrarianId",
                table: "Loans",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "BorrowDate",
                table: "Loans",
                newName: "CheckOutDueDate");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_CheckOutLibrarianId",
                table: "Loans",
                newName: "IX_Loans_UserId");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Books",
                newName: "BorrowCartId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_ReservationId",
                table: "Books",
                newName: "IX_Books_BorrowCartId");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "BorrowCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowCarts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckOutRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    LibrarianId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOutRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckOutRecords_Librarians_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "Librarians",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckOutRecords_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    LibrarianId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnRecords_Librarians_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "Librarians",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReturnRecords_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorrowCarts_UserId",
                table: "BorrowCarts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckOutRecords_LibrarianId",
                table: "CheckOutRecords",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOutRecords_LoanId",
                table: "CheckOutRecords",
                column: "LoanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRecords_LibrarianId",
                table: "ReturnRecords",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRecords_LoanId",
                table: "ReturnRecords",
                column: "LoanId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BorrowCarts_BorrowCartId",
                table: "Books",
                column: "BorrowCartId",
                principalTable: "BorrowCarts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_BorrowCarts_BorrowCartId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "BorrowCarts");

            migrationBuilder.DropTable(
                name: "CheckOutRecords");

            migrationBuilder.DropTable(
                name: "ReturnRecords");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Loans",
                newName: "CheckOutLibrarianId");

            migrationBuilder.RenameColumn(
                name: "LoanCreationDate",
                table: "Loans",
                newName: "ReturnDate");

            migrationBuilder.RenameColumn(
                name: "CheckOutDueDate",
                table: "Loans",
                newName: "BorrowDate");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                newName: "IX_Loans_CheckOutLibrarianId");

            migrationBuilder.RenameColumn(
                name: "BorrowCartId",
                table: "Books",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BorrowCartId",
                table: "Books",
                newName: "IX_Books_ReservationId");

            migrationBuilder.AddColumn<int>(
                name: "CheckInLibrarianId",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservationDueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CheckInLibrarianId",
                table: "Loans",
                column: "CheckInLibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LoanId",
                table: "Reservations",
                column: "LoanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Reservations_ReservationId",
                table: "Books",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Librarians_CheckInLibrarianId",
                table: "Loans",
                column: "CheckInLibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Librarians_CheckOutLibrarianId",
                table: "Loans",
                column: "CheckOutLibrarianId",
                principalTable: "Librarians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
