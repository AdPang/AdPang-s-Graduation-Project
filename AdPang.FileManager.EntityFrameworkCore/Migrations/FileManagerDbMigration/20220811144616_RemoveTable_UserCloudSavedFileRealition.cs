using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdPang.FileManager.EntityFrameworkCore.Migrations.FileManagerDbMigration
{
    public partial class RemoveTable_UserCloudSavedFileRealition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCloudSavedFileRealition_AspNetUsers_UsersId",
                table: "UserCloudSavedFileRealition");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCloudSavedFileRealition_CloudFileInfos_CloudFileInfosId",
                table: "UserCloudSavedFileRealition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCloudSavedFileRealition",
                table: "UserCloudSavedFileRealition");

            migrationBuilder.RenameTable(
                name: "UserCloudSavedFileRealition",
                newName: "CloudFileInfoUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserCloudSavedFileRealition_UsersId",
                table: "CloudFileInfoUser",
                newName: "IX_CloudFileInfoUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudFileInfoUser",
                table: "CloudFileInfoUser",
                columns: new[] { "CloudFileInfosId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CloudFileInfoUser_AspNetUsers_UsersId",
                table: "CloudFileInfoUser",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CloudFileInfoUser_CloudFileInfos_CloudFileInfosId",
                table: "CloudFileInfoUser",
                column: "CloudFileInfosId",
                principalTable: "CloudFileInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudFileInfoUser_AspNetUsers_UsersId",
                table: "CloudFileInfoUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CloudFileInfoUser_CloudFileInfos_CloudFileInfosId",
                table: "CloudFileInfoUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CloudFileInfoUser",
                table: "CloudFileInfoUser");

            migrationBuilder.RenameTable(
                name: "CloudFileInfoUser",
                newName: "UserCloudSavedFileRealition");

            migrationBuilder.RenameIndex(
                name: "IX_CloudFileInfoUser_UsersId",
                table: "UserCloudSavedFileRealition",
                newName: "IX_UserCloudSavedFileRealition_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCloudSavedFileRealition",
                table: "UserCloudSavedFileRealition",
                columns: new[] { "CloudFileInfosId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCloudSavedFileRealition_AspNetUsers_UsersId",
                table: "UserCloudSavedFileRealition",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCloudSavedFileRealition_CloudFileInfos_CloudFileInfosId",
                table: "UserCloudSavedFileRealition",
                column: "CloudFileInfosId",
                principalTable: "CloudFileInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
