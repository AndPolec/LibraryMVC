using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class LoanUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "TotalAmountOfPaidPenalty",
                table: "ReturnRecords",
                newName: "TotalPenalty");

            migrationBuilder.AddColumn<decimal>(
                name: "OverduePenalty",
                table: "ReturnRecords",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "isOverdue",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isBlocked",
                table: "LibraryUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverduePenalty",
                table: "ReturnRecords");

            migrationBuilder.DropColumn(
                name: "isOverdue",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "isBlocked",
                table: "LibraryUsers");

            migrationBuilder.RenameColumn(
                name: "TotalPenalty",
                table: "ReturnRecords",
                newName: "TotalAmountOfPaidPenalty");

            migrationBuilder.AddColumn<decimal>(
                name: "Penalty",
                table: "Loans",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
