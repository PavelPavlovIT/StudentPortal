using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class AddIndexToGroupStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudentId_GroupId",
                table: "GroupStudents",
                columns: new[] { "StudentId", "GroupId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_StudentId_GroupId",
                table: "GroupStudents");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents",
                column: "StudentId");
        }
    }
}
