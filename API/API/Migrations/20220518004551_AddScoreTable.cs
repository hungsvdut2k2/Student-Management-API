using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddScoreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseClassroomId = table.Column<int>(type: "int", nullable: false),
                    excerciseScore = table.Column<double>(type: "float", nullable: false),
                    midTermScore = table.Column<double>(type: "float", nullable: false),
                    finalTermScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Score_CoursesClassroom_CourseClassroomId",
                        column: x => x.CourseClassroomId,
                        principalTable: "CoursesClassroom",
                        principalColumn: "CourseClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Score_UsersInformation_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UsersInformation",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Score_CourseClassroomId",
                table: "Score",
                column: "CourseClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_UserInformationId",
                table: "Score",
                column: "UserInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Score");
        }
    }
}
