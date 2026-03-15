using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlAshmar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Halaqas",
                newName: "HalaqaName");

            migrationBuilder.RenameColumn(
                name: "EventName",
                table: "Courses",
                newName: "CourseName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HalaqaName",
                table: "Halaqas",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "Courses",
                newName: "EventName");
        }
    }
}
