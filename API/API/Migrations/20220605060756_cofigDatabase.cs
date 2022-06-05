using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class cofigDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEducationProgram_Courses_EducationalProgramId",
                table: "CourseEducationProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEducationProgram_EducationalProgram_CourseId",
                table: "CourseEducationProgram");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEducationProgram_Courses_CourseId",
                table: "CourseEducationProgram",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEducationProgram_EducationalProgram_EducationalProgramId",
                table: "CourseEducationProgram",
                column: "EducationalProgramId",
                principalTable: "EducationalProgram",
                principalColumn: "EducationalProgramId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEducationProgram_Courses_CourseId",
                table: "CourseEducationProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEducationProgram_EducationalProgram_EducationalProgramId",
                table: "CourseEducationProgram");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEducationProgram_Courses_EducationalProgramId",
                table: "CourseEducationProgram",
                column: "EducationalProgramId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEducationProgram_EducationalProgram_CourseId",
                table: "CourseEducationProgram",
                column: "CourseId",
                principalTable: "EducationalProgram",
                principalColumn: "EducationalProgramId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
