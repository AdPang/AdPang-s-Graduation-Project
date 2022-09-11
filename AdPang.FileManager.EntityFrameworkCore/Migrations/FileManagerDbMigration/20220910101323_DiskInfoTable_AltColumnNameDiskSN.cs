using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class DiskInfoTable_AltColumnNameDiskSN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "char",
                table: "PrivateDiskInfos",
                newName: "DiskSN");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateDiskInfos_char",
                table: "PrivateDiskInfos",
                newName: "IX_PrivateDiskInfos_DiskSN");

            migrationBuilder.AlterColumn<string>(
                name: "DiskSN",
                table: "PrivateDiskInfos",
                type: "char(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiskSN",
                table: "PrivateDiskInfos",
                newName: "char");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateDiskInfos_DiskSN",
                table: "PrivateDiskInfos",
                newName: "IX_PrivateDiskInfos_char");

            migrationBuilder.AlterColumn<string>(
                name: "char",
                table: "PrivateDiskInfos",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(64)",
                oldMaxLength: 64);
        }
    }
}
