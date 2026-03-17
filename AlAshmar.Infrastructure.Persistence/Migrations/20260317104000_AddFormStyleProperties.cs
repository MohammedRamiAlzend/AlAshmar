using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlAshmar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFormStyleProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryColor",
                table: "Forms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Forms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontFamily",
                table: "Forms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColumnSpan",
                table: "FormQuestions",
                type: "int",
                nullable: false,
                defaultValue: 12);

            migrationBuilder.AddColumn<string>(
                name: "LabelColor",
                table: "FormQuestions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FontSize",
                table: "FormQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontFamily",
                table: "FormQuestions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryColor",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "FontFamily",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ColumnSpan",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "LabelColor",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "FontSize",
                table: "FormQuestions");

            migrationBuilder.DropColumn(
                name: "FontFamily",
                table: "FormQuestions");
        }
    }
}
