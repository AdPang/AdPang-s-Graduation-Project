using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class AddTable_SharedFileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharedFileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedDesc = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    SharedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShardByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SingleFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSingleFile = table.Column<bool>(type: "bit", nullable: false),
                    CreatTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedFileInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedFileInfos_AspNetUsers_ShardByUserId",
                        column: x => x.ShardByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SharedFileInfos_DirInfos_DirId",
                        column: x => x.DirId,
                        principalTable: "DirInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SharedFileInfos_UserPrivateFileInfos_SingleFileId",
                        column: x => x.SingleFileId,
                        principalTable: "UserPrivateFileInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedFileInfos_DirId",
                table: "SharedFileInfos",
                column: "DirId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedFileInfos_ShardByUserId",
                table: "SharedFileInfos",
                column: "ShardByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedFileInfos_SingleFileId",
                table: "SharedFileInfos",
                column: "SingleFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedFileInfos");
        }
    }
}
