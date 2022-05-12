using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddFacultyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "UsersInformation");

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Classrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_FacultyId",
                table: "Classrooms",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Faculty_FacultyId",
                table: "Classrooms",
                column: "FacultyId",
                principalTable: "Faculty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Faculty_FacultyId",
                table: "Classrooms");

            migrationBuilder.DropTable(
                name: "Faculty");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_FacultyId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Classrooms");

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "UsersInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
