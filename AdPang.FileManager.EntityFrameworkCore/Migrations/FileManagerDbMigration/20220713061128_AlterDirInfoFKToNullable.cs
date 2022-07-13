using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class AlterDirInfoFKToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirInfos_DirInfos_ParentDirInfoId",
                table: "DirInfos");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentDirInfoId",
                table: "DirInfos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_DirInfos_DirInfos_ParentDirInfoId",
                table: "DirInfos",
                column: "ParentDirInfoId",
                principalTable: "DirInfos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirInfos_DirInfos_ParentDirInfoId",
                table: "DirInfos");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentDirInfoId",
                table: "DirInfos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DirInfos_DirInfos_ParentDirInfoId",
                table: "DirInfos",
                column: "ParentDirInfoId",
                principalTable: "DirInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
