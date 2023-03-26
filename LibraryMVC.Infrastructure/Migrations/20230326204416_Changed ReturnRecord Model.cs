using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class ChangedReturnRecordModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalPenaltyForLostAndDestroyedBooks",
                table: "ReturnRecords",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "ReturnRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPenaltyPaid",
                table: "ReturnRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountOfPaidPenalty",
                table: "ReturnRecords",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ReturnRecordLostOrDestroyedBooks",
                columns: table => new
                {
                    LostOrDestroyedBooksId = table.Column<int>(type: "int", nullable: false),
                    ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRecordLostOrDestroyedBooks", x => new { x.LostOrDestroyedBooksId, x.ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId });
                    table.ForeignKey(
                        name: "FK_ReturnRecordLostOrDestroyedBooks_Books_LostOrDestroyedBooksId",
                        column: x => x.LostOrDestroyedBooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnRecordLostOrDestroyedBooks_ReturnRecords_ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId",
                        column: x => x.ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId,
                        principalTable: "ReturnRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRecordReturnedBooks",
                columns: table => new
                {
                    ReturnRecordsWhereCopyOfBookWasReturnedId = table.Column<int>(type: "int", nullable: false),
                    ReturnedBooksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRecordReturnedBooks", x => new { x.ReturnRecordsWhereCopyOfBookWasReturnedId, x.ReturnedBooksId });
                    table.ForeignKey(
                        name: "FK_ReturnRecordReturnedBooks_Books_ReturnedBooksId",
                        column: x => x.ReturnedBooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnRecordReturnedBooks_ReturnRecords_ReturnRecordsWhereCopyOfBookWasReturnedId",
                        column: x => x.ReturnRecordsWhereCopyOfBookWasReturnedId,
                        principalTable: "ReturnRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRecordLostOrDestroyedBooks_ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId",
                table: "ReturnRecordLostOrDestroyedBooks",
                column: "ReturnRecordsWhereCopyOfBookWasLostOrDestroyedId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRecordReturnedBooks_ReturnedBooksId",
                table: "ReturnRecordReturnedBooks",
                column: "ReturnedBooksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnRecordLostOrDestroyedBooks");

            migrationBuilder.DropTable(
                name: "ReturnRecordReturnedBooks");

            migrationBuilder.DropColumn(
                name: "AdditionalPenaltyForLostAndDestroyedBooks",
                table: "ReturnRecords");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "ReturnRecords");

            migrationBuilder.DropColumn(
                name: "IsPenaltyPaid",
                table: "ReturnRecords");

            migrationBuilder.DropColumn(
                name: "TotalAmountOfPaidPenalty",
                table: "ReturnRecords");
        }
    }
}
