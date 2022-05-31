using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class configDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dob",
                table: "UsersInformation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "EducationalProgramId",
                table: "Classrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_EducationalProgramId",
                table: "Classrooms",
                column: "EducationalProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_EducationalProgram_EducationalProgramId",
                table: "Classrooms",
                column: "EducationalProgramId",
                principalTable: "EducationalProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_EducationalProgram_EducationalProgramId",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_EducationalProgramId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "EducationalProgramId",
                table: "Classrooms");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Dob",
                table: "UsersInformation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
