using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.MigrationDb.LogDbMigration
{
    public partial class ExceptionLogTable_AlterColum_ExceptionType_RemoveRequesetParameterColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestParameter",
                table: "ExceptionLog",
                newName: "ExceptionType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExceptionType",
                table: "ExceptionLog",
                newName: "RequestParameter");
        }
    }
}
