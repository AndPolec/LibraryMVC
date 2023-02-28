using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class UpdatedRelationBetweenBookLoanBorrowingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BorrowingCarts_BorrowingCartId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowingCartId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowingCartId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookBorrowingCart",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    BorrowingCartsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingCart", x => new { x.BooksId, x.BorrowingCartsId });
                    table.ForeignKey(
                        name: "FK_BookBorrowingCart_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBorrowingCart_BorrowingCarts_BorrowingCartsId",
                        column: x => x.BorrowingCartsId,
                        principalTable: "BorrowingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookLoan",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    LoansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLoan", x => new { x.BooksId, x.LoansId });
                    table.ForeignKey(
                        name: "FK_BookLoan_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLoan_Loans_LoansId",
                        column: x => x.LoansId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingCart_BorrowingCartsId",
                table: "BookBorrowingCart",
                column: "BorrowingCartsId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLoan_LoansId",
                table: "BookLoan",
                column: "LoansId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrowingCart");

            migrationBuilder.DropTable(
                name: "BookLoan");

            migrationBuilder.AddColumn<int>(
                name: "BorrowingCartId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowingCartId",
                table: "Books",
                column: "BorrowingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BorrowingCarts_BorrowingCartId",
                table: "Books",
                column: "BorrowingCartId",
                principalTable: "BorrowingCarts",
                principalColumn: "Id");
        }
    }
}
