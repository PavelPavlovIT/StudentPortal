using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class updateGroupStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "GroupStudents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_GroupId1",
                table: "GroupStudents",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_Groups_GroupId1",
                table: "GroupStudents",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_Students_StudentId",
                table: "GroupStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_Groups_GroupId1",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_Students_StudentId",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_GroupId1",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "GroupStudents");
        }
    }
}
