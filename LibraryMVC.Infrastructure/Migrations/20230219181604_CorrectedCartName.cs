using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class CorrectedCartName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BorrowCarts_BorrowCartId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BorrowCarts");

            migrationBuilder.RenameColumn(
                name: "BorrowCartId",
                table: "Books",
                newName: "BorrowingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BorrowCartId",
                table: "Books",
                newName: "IX_Books_BorrowingCartId");

            migrationBuilder.CreateTable(
                name: "BorrowingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingCarts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingCarts_UserId",
                table: "BorrowingCarts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BorrowingCarts_BorrowingCartId",
                table: "Books",
                column: "BorrowingCartId",
                principalTable: "BorrowingCarts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BorrowingCarts_BorrowingCartId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BorrowingCarts");

            migrationBuilder.RenameColumn(
                name: "BorrowingCartId",
                table: "Books",
                newName: "BorrowCartId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BorrowingCartId",
                table: "Books",
                newName: "IX_Books_BorrowCartId");

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

            migrationBuilder.CreateIndex(
                name: "IX_BorrowCarts_UserId",
                table: "BorrowCarts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BorrowCarts_BorrowCartId",
                table: "Books",
                column: "BorrowCartId",
                principalTable: "BorrowCarts",
                principalColumn: "Id");
        }
    }
}
