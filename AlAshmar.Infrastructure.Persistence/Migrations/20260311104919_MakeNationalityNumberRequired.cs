using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlAshmar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeNationalityNumberRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_NationalityNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_NationalityNumber",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "NationalityNumber",
                table: "Teachers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalityNumber",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_NationalityNumber",
                table: "Teachers",
                column: "NationalityNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_NationalityNumber",
                table: "Students",
                column: "NationalityNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_NationalityNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_NationalityNumber",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "NationalityNumber",
                table: "Teachers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NationalityNumber",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_NationalityNumber",
                table: "Teachers",
                column: "NationalityNumber",
                unique: true,
                filter: "[NationalityNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_NationalityNumber",
                table: "Students",
                column: "NationalityNumber",
                unique: true,
                filter: "[NationalityNumber] IS NOT NULL");
        }
    }
}
