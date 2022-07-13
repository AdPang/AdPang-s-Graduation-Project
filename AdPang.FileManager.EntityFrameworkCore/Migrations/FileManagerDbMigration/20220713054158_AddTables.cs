using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudFileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileLength = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    @char = table.Column<string>(name: "char", type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFileInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudFileInfos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DirInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentDirInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirInfos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirInfos_DirInfos_ParentDirInfoId",
                        column: x => x.ParentDirInfoId,
                        principalTable: "DirInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCloudSavedFileRealition",
                columns: table => new
                {
                    CloudFileInfosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCloudSavedFileRealition", x => new { x.CloudFileInfosId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserCloudSavedFileRealition_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCloudSavedFileRealition_CloudFileInfos_CloudFileInfosId",
                        column: x => x.CloudFileInfosId,
                        principalTable: "CloudFileInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPrivateFileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentDirectoryInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrivateFileInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrivateFileInfos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPrivateFileInfos_DirInfos_CurrentDirectoryInfoId",
                        column: x => x.CurrentDirectoryInfoId,
                        principalTable: "DirInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudFileInfos_UserId",
                table: "CloudFileInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DirInfos_ParentDirInfoId",
                table: "DirInfos",
                column: "ParentDirInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DirInfos_UserId",
                table: "DirInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCloudSavedFileRealition_UsersId",
                table: "UserCloudSavedFileRealition",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivateFileInfos_CurrentDirectoryInfoId",
                table: "UserPrivateFileInfos",
                column: "CurrentDirectoryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivateFileInfos_UserId",
                table: "UserPrivateFileInfos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCloudSavedFileRealition");

            migrationBuilder.DropTable(
                name: "UserPrivateFileInfos");

            migrationBuilder.DropTable(
                name: "CloudFileInfos");

            migrationBuilder.DropTable(
                name: "DirInfos");
        }
    }
}
