using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addClassroomAndFacultyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassroomId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "Classroom",
                columns: table => new
                {
                    ClassroomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicYear = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EducationalProgramId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classroom", x => x.ClassroomId);
                    table.ForeignKey(
                        name: "FK_Classroom_EducationalProgram_EducationalProgramId",
                        column: x => x.EducationalProgramId,
                        principalTable: "EducationalProgram",
                        principalColumn: "EducationalProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classroom_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ClassroomId",
                table: "User",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Classroom_EducationalProgramId",
                table: "Classroom",
                column: "EducationalProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Classroom_FacultyId",
                table: "Classroom",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Classroom_ClassroomId",
                table: "User",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "ClassroomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Classroom_ClassroomId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Classroom");

            migrationBuilder.DropTable(
                name: "Faculty");

            migrationBuilder.DropIndex(
                name: "IX_User_ClassroomId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "User");
        }
    }
}
