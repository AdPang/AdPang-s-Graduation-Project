using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class DiskInfoTable_AddDriveNameColumn_AddComputerName_AltDiskSNUnique10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComputerName",
                table: "PrivateDiskInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveName",
                table: "PrivateDiskInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateDiskInfos_char",
                table: "PrivateDiskInfos",
                column: "char",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PrivateDiskInfos_char",
                table: "PrivateDiskInfos");

            migrationBuilder.DropColumn(
                name: "ComputerName",
                table: "PrivateDiskInfos");

            migrationBuilder.DropColumn(
                name: "DriveName",
                table: "PrivateDiskInfos");
        }
    }
}
