using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class ConfigManyToManyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEducationalProgram");

            migrationBuilder.CreateTable(
                name: "CourseEducationalPrograms",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EducationalProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEducationalPrograms", x => new { x.CourseId, x.EducationalProgramId });
                    table.ForeignKey(
                        name: "FK_CourseEducationalPrograms_Courses_EducationalProgramId",
                        column: x => x.EducationalProgramId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEducationalPrograms_EducationalProgram_CourseId",
                        column: x => x.CourseId,
                        principalTable: "EducationalProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEducationalPrograms_EducationalProgramId",
                table: "CourseEducationalPrograms",
                column: "EducationalProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEducationalPrograms");

            migrationBuilder.CreateTable(
                name: "CourseEducationalProgram",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    EducationalProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEducationalProgram", x => new { x.CoursesId, x.EducationalProgramId });
                    table.ForeignKey(
                        name: "FK_CourseEducationalProgram_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEducationalProgram_EducationalProgram_EducationalProgramId",
                        column: x => x.EducationalProgramId,
                        principalTable: "EducationalProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEducationalProgram_EducationalProgramId",
                table: "CourseEducationalProgram",
                column: "EducationalProgramId");
        }
    }
}
