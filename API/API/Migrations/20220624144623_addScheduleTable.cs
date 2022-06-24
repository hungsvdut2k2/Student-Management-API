using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addScheduleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateInWeek = table.Column<int>(type: "int", nullable: false),
                    startPeriod = table.Column<int>(type: "int", nullable: false),
                    endPeriod = table.Column<int>(type: "int", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseClassroomCourseClassId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseClassId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_CourseClassroom_CourseClassroomCourseClassId",
                        column: x => x.CourseClassroomCourseClassId,
                        principalTable: "CourseClassroom",
                        principalColumn: "CourseClassId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_CourseClassroomCourseClassId",
                table: "Schedule",
                column: "CourseClassroomCourseClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");
        }
    }
}
