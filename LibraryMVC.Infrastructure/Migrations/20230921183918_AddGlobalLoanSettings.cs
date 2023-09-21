using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class AddGlobalLoanSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalLoanSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverduePenaltyRatePerDayForOneBook = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DurationOfFreeLoanInDays = table.Column<int>(type: "int", nullable: false),
                    MaxBooksInOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalLoanSettings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Administrator",
                column: "ConcurrencyStamp",
                value: "4a2853ba-219f-4b2f-b332-28a647441419");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Librarian",
                column: "ConcurrencyStamp",
                value: "a79debd5-3c15-47bb-8b34-5d936da7634b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Reader",
                column: "ConcurrencyStamp",
                value: "13a29f80-36c4-410a-8c4f-db51008bee29");

            migrationBuilder.InsertData(
                table: "GlobalLoanSettings",
                columns: new[] { "Id", "DurationOfFreeLoanInDays", "MaxBooksInOrder", "OverduePenaltyRatePerDayForOneBook" },
                values: new object[] { 1, 21, 5, 0.2m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalLoanSettings");

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
        }
    }
}
