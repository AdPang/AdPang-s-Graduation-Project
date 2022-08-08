using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class AddColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RealFileInfoId",
                table: "UserPrivateFileInfos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DirName",
                table: "DirInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivateFileInfos_RealFileInfoId",
                table: "UserPrivateFileInfos",
                column: "RealFileInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPrivateFileInfos_CloudFileInfos_RealFileInfoId",
                table: "UserPrivateFileInfos",
                column: "RealFileInfoId",
                principalTable: "CloudFileInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPrivateFileInfos_CloudFileInfos_RealFileInfoId",
                table: "UserPrivateFileInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserPrivateFileInfos_RealFileInfoId",
                table: "UserPrivateFileInfos");

            migrationBuilder.DropColumn(
                name: "RealFileInfoId",
                table: "UserPrivateFileInfos");

            migrationBuilder.DropColumn(
                name: "DirName",
                table: "DirInfos");
        }
    }
}
