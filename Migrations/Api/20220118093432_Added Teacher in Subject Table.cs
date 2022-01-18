using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityManagementBackend.Migrations.Api
{
    public partial class AddedTeacherinSubjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "subjects");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "subjects");
        }
    }
}
