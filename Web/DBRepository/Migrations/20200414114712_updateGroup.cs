using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class updateGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_Groups_GroupId1",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_GroupId1",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "GroupStudents");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "GroupStudents",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_GroupId",
                table: "GroupStudents",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_Groups_GroupId",
                table: "GroupStudents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_Groups_GroupId",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_GroupId",
                table: "GroupStudents");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "GroupStudents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "GroupStudents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_GroupId1",
                table: "GroupStudents",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_Groups_GroupId1",
                table: "GroupStudents",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
