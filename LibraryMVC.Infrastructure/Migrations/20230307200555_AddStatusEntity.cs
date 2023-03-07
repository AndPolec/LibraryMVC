using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class AddStatusEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckOutDueDate",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "LoanCreationDate",
                table: "Loans",
                newName: "CreationDate");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nowe" },
                    { 2, "Wypożyczone" },
                    { 3, "Zakończone" },
                    { 4, "Zaległe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_StatusId",
                table: "Loans",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Statuses_StatusId",
                table: "Loans",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Statuses_StatusId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Loans_StatusId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Loans",
                newName: "LoanCreationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutDueDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
