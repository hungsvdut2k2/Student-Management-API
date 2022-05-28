using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addColumnsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "midTermScore",
                table: "Score",
                newName: "MidTermScore");

            migrationBuilder.RenameColumn(
                name: "midTermRate",
                table: "Score",
                newName: "MidTermRate");

            migrationBuilder.RenameColumn(
                name: "finalTermScore",
                table: "Score",
                newName: "FinalTermScore");

            migrationBuilder.RenameColumn(
                name: "finalTermRate",
                table: "Score",
                newName: "FinalTermRate");

            migrationBuilder.RenameColumn(
                name: "excerciseScore",
                table: "Score",
                newName: "ExcerciseScore");

            migrationBuilder.RenameColumn(
                name: "excerciseRate",
                table: "Score",
                newName: "ExcerciseRate");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "UsersInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "UsersInformation");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "MidTermScore",
                table: "Score",
                newName: "midTermScore");

            migrationBuilder.RenameColumn(
                name: "MidTermRate",
                table: "Score",
                newName: "midTermRate");

            migrationBuilder.RenameColumn(
                name: "FinalTermScore",
                table: "Score",
                newName: "finalTermScore");

            migrationBuilder.RenameColumn(
                name: "FinalTermRate",
                table: "Score",
                newName: "finalTermRate");

            migrationBuilder.RenameColumn(
                name: "ExcerciseScore",
                table: "Score",
                newName: "excerciseScore");

            migrationBuilder.RenameColumn(
                name: "ExcerciseRate",
                table: "Score",
                newName: "excerciseRate");
        }
    }
}
