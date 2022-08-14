using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class AddColumn_ExpiredFromTableSharedFileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredTime",
                table: "SharedFileInfos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "HasExpired",
                table: "SharedFileInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredTime",
                table: "SharedFileInfos");

            migrationBuilder.DropColumn(
                name: "HasExpired",
                table: "SharedFileInfos");
        }
    }
}
