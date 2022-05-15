using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class configManyToManyRelationshipForCourseClassroomAndUserInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseClassroomUserInformation");

            migrationBuilder.CreateTable(
                name: "CourseClassroomUserInformations",
                columns: table => new
                {
                    CourseClassId = table.Column<int>(type: "int", nullable: false),
                    UserInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClassroomUserInformations", x => new { x.CourseClassId, x.UserInformationId });
                    table.ForeignKey(
                        name: "FK_CourseClassroomUserInformations_CoursesClassroom_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "CoursesClassroom",
                        principalColumn: "CourseClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseClassroomUserInformations_UsersInformation_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UsersInformation",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroomUserInformations_UserInformationId",
                table: "CourseClassroomUserInformations",
                column: "UserInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseClassroomUserInformations");

            migrationBuilder.CreateTable(
                name: "CourseClassroomUserInformation",
                columns: table => new
                {
                    CourseClassroomsCourseClassroomId = table.Column<int>(type: "int", nullable: false),
                    UserInformationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClassroomUserInformation", x => new { x.CourseClassroomsCourseClassroomId, x.UserInformationUserId });
                    table.ForeignKey(
                        name: "FK_CourseClassroomUserInformation_CoursesClassroom_CourseClassroomsCourseClassroomId",
                        column: x => x.CourseClassroomsCourseClassroomId,
                        principalTable: "CoursesClassroom",
                        principalColumn: "CourseClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseClassroomUserInformation_UsersInformation_UserInformationUserId",
                        column: x => x.UserInformationUserId,
                        principalTable: "UsersInformation",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroomUserInformation_UserInformationUserId",
                table: "CourseClassroomUserInformation",
                column: "UserInformationUserId");
        }
    }
}
