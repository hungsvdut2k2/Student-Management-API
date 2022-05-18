using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddMoreAttributeForCalculateScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "excerciseRate",
                table: "Score",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "finalTermRate",
                table: "Score",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "midTermRate",
                table: "Score",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "excerciseRate",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "finalTermRate",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "midTermRate",
                table: "Score");
        }
    }
}
