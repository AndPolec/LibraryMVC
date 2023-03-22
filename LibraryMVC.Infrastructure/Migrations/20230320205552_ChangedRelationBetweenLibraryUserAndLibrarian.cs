using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMVC.Infrastructure.Migrations
{
    public partial class ChangedRelationBetweenLibraryUserAndLibrarian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOutRecords_LibraryUsers_LibrarianId",
                table: "CheckOutRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRecords_LibraryUsers_LibrarianId",
                table: "ReturnRecords");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LibraryUsers");

            migrationBuilder.RenameColumn(
                name: "LibrarianId",
                table: "ReturnRecords",
                newName: "AdditionalLibrarianInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnRecords_LibrarianId",
                table: "ReturnRecords",
                newName: "IX_ReturnRecords_AdditionalLibrarianInfoId");

            migrationBuilder.RenameColumn(
                name: "LibrarianId",
                table: "CheckOutRecords",
                newName: "AdditionalLibrarianInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckOutRecords_LibrarianId",
                table: "CheckOutRecords",
                newName: "IX_CheckOutRecords_AdditionalLibrarianInfoId");

            migrationBuilder.CreateTable(
                name: "AdditionalLibrarianInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalLibrarianInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalLibrarianInfo_LibraryUsers_LibraryUserId",
                        column: x => x.LibraryUserId,
                        principalTable: "LibraryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Anulowane" });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalLibrarianInfo_LibraryUserId",
                table: "AdditionalLibrarianInfo",
                column: "LibraryUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOutRecords_AdditionalLibrarianInfo_AdditionalLibrarianInfoId",
                table: "CheckOutRecords",
                column: "AdditionalLibrarianInfoId",
                principalTable: "AdditionalLibrarianInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRecords_AdditionalLibrarianInfo_AdditionalLibrarianInfoId",
                table: "ReturnRecords",
                column: "AdditionalLibrarianInfoId",
                principalTable: "AdditionalLibrarianInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOutRecords_AdditionalLibrarianInfo_AdditionalLibrarianInfoId",
                table: "CheckOutRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRecords_AdditionalLibrarianInfo_AdditionalLibrarianInfoId",
                table: "ReturnRecords");

            migrationBuilder.DropTable(
                name: "AdditionalLibrarianInfo");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "AdditionalLibrarianInfoId",
                table: "ReturnRecords",
                newName: "LibrarianId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnRecords_AdditionalLibrarianInfoId",
                table: "ReturnRecords",
                newName: "IX_ReturnRecords_LibrarianId");

            migrationBuilder.RenameColumn(
                name: "AdditionalLibrarianInfoId",
                table: "CheckOutRecords",
                newName: "LibrarianId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckOutRecords_AdditionalLibrarianInfoId",
                table: "CheckOutRecords",
                newName: "IX_CheckOutRecords_LibrarianId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LibraryUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOutRecords_LibraryUsers_LibrarianId",
                table: "CheckOutRecords",
                column: "LibrarianId",
                principalTable: "LibraryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRecords_LibraryUsers_LibrarianId",
                table: "ReturnRecords",
                column: "LibrarianId",
                principalTable: "LibraryUsers",
                principalColumn: "Id");
        }
    }
}
