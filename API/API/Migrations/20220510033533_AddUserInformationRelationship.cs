using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddUserInformationRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserInformationId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UsersInformation",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.Now),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInformation", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserInformationId",
                table: "Users",
                column: "UserInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UsersInformation_UserInformationId",
                table: "Users",
                column: "UserInformationId",
                principalTable: "UsersInformation",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UsersInformation_UserInformationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UsersInformation");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserInformationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserInformationId",
                table: "Users");
        }
    }
}
