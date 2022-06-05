using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class configRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseClassroom_Courses_CourseId1",
                table: "CourseClassroom");

            migrationBuilder.DropIndex(
                name: "IX_CourseClassroom_CourseId1",
                table: "CourseClassroom");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "CourseClassroom");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "CourseClassroom",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_CourseId",
                table: "CourseClassroom",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseClassroom_Courses_CourseId",
                table: "CourseClassroom",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseClassroom_Courses_CourseId",
                table: "CourseClassroom");

            migrationBuilder.DropIndex(
                name: "IX_CourseClassroom_CourseId",
                table: "CourseClassroom");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseClassroom",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CourseId1",
                table: "CourseClassroom",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_CourseId1",
                table: "CourseClassroom",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseClassroom_Courses_CourseId1",
                table: "CourseClassroom",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }
    }
}
