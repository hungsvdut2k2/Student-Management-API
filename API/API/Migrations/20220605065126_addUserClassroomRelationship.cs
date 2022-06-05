using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addUserClassroomRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Classroom_ClassroomId",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "ClassroomId",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Classroom_ClassroomId",
                table: "User",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "ClassroomId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Classroom_ClassroomId",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "ClassroomId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Classroom_ClassroomId",
                table: "User",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "ClassroomId");
        }
    }
}
