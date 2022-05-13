using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddCoursesClassroomRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEducationalProgram_Courses_CoursesCourseId",
                table: "CourseEducationalProgram");

            migrationBuilder.DropTable(
                name: "CourseUserInformation");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Courses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CoursesCourseId",
                table: "CourseEducationalProgram",
                newName: "CoursesId");

            migrationBuilder.CreateTable(
                name: "CoursesClassroom",
                columns: table => new
                {
                    CourseClassroomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesClassroom", x => x.CourseClassroomId);
                    table.ForeignKey(
                        name: "FK_CoursesClassroom_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CoursesClassroom_CourseId",
                table: "CoursesClassroom",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEducationalProgram_Courses_CoursesId",
                table: "CourseEducationalProgram",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEducationalProgram_Courses_CoursesId",
                table: "CourseEducationalProgram");

            migrationBuilder.DropTable(
                name: "CourseClassroomUserInformation");

            migrationBuilder.DropTable(
                name: "CoursesClassroom");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Courses",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "CoursesId",
                table: "CourseEducationalProgram",
                newName: "CoursesCourseId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Schedule",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CourseUserInformation",
                columns: table => new
                {
                    CoursesCourseId = table.Column<int>(type: "int", nullable: false),
                    UserInformationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUserInformation", x => new { x.CoursesCourseId, x.UserInformationUserId });
                    table.ForeignKey(
                        name: "FK_CourseUserInformation_Courses_CoursesCourseId",
                        column: x => x.CoursesCourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUserInformation_UsersInformation_UserInformationUserId",
                        column: x => x.UserInformationUserId,
                        principalTable: "UsersInformation",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUserInformation_UserInformationUserId",
                table: "CourseUserInformation",
                column: "UserInformationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEducationalProgram_Courses_CoursesCourseId",
                table: "CourseEducationalProgram",
                column: "CoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
