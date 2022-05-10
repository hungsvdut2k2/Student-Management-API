using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddEducationalRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationalProgramId",
                table: "UsersInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EducationalProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseEducationalProgram",
                columns: table => new
                {
                    CoursesCourseId = table.Column<int>(type: "int", nullable: false),
                    EducationalProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEducationalProgram", x => new { x.CoursesCourseId, x.EducationalProgramId });
                    table.ForeignKey(
                        name: "FK_CourseEducationalProgram_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEducationalProgram");

            migrationBuilder.DropTable(
                name: "EducationalProgram");

            migrationBuilder.DropColumn(
                name: "EducationalProgramId",
                table: "UsersInformation");
        }
    }
}
