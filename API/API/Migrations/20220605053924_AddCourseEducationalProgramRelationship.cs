using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddCourseEducationalProgramRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationalProgram",
                columns: table => new
                {
                    EducationalProgramId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalProgram", x => x.EducationalProgramId);
                });

            migrationBuilder.CreateTable(
                name: "CourseEducationProgram",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EducationalProgramId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEducationProgram", x => new { x.CourseId, x.EducationalProgramId });
                    table.ForeignKey(
                        name: "FK_CourseEducationProgram_Courses_EducationalProgramId",
                        column: x => x.EducationalProgramId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEducationProgram_EducationalProgram_CourseId",
                        column: x => x.CourseId,
                        principalTable: "EducationalProgram",
                        principalColumn: "EducationalProgramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEducationProgram_EducationalProgramId",
                table: "CourseEducationProgram",
                column: "EducationalProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEducationProgram");

            migrationBuilder.DropTable(
                name: "EducationalProgram");
        }
    }
}
