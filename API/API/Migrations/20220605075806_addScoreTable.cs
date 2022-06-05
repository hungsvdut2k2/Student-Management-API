using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addScoreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseClassroomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExcerciseScore = table.Column<double>(type: "float", nullable: false),
                    MidTermScore = table.Column<double>(type: "float", nullable: false),
                    FinalTermScore = table.Column<double>(type: "float", nullable: false),
                    ExcerciseRate = table.Column<double>(type: "float", nullable: false),
                    MidTermRate = table.Column<double>(type: "float", nullable: false),
                    FinalTermRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Score_CourseClassroom_CourseClassroomId",
                        column: x => x.CourseClassroomId,
                        principalTable: "CourseClassroom",
                        principalColumn: "CourseClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Score_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Score_CourseClassroomId",
                table: "Score",
                column: "CourseClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_UserId",
                table: "Score",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Score");
        }
    }
}
