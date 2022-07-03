using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.MigrationDb.FileManager
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestTable_UserId",
                table: "TestTable");

            migrationBuilder.CreateIndex(
                name: "IX_TestTable_UserId",
                table: "TestTable",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestTable_UserId",
                table: "TestTable");

            migrationBuilder.CreateIndex(
                name: "IX_TestTable_UserId",
                table: "TestTable",
                column: "UserId");
        }
    }
}
