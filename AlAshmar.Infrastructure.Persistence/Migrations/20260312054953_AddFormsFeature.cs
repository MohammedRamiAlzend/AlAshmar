using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlAshmar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFormsFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FormType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Audience = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccessToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimerMinutes = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowMultipleResponses = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    StartsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByTeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HalaqaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Forms_Halaqas_HalaqaId",
                        column: x => x.HalaqaId,
                        principalTable: "Halaqas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Forms_Managers_CreatedByManagerId",
                        column: x => x.CreatedByManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Forms_Teachers_CreatedByTeacherId",
                        column: x => x.CreatedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FormQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    QuestionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Points = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormQuestions_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RespondedByStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RespondedByTeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSpentSeconds = table.Column<int>(type: "int", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TotalScore = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormResponses_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormResponses_Students_RespondedByStudentId",
                        column: x => x.RespondedByStudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FormResponses_Teachers_RespondedByTeacherId",
                        column: x => x.RespondedByTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FormQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormQuestionOptions_FormQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "FormQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextAnswer = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    PointsAwarded = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormAnswers_FormQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "FormQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormAnswers_FormResponses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "FormResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormAnswerSelectedOptions",
                columns: table => new
                {
                    FormAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormQuestionOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAnswerSelectedOptions", x => new { x.FormAnswerId, x.FormQuestionOptionId });
                    table.ForeignKey(
                        name: "FK_FormAnswerSelectedOptions_FormAnswers_FormAnswerId",
                        column: x => x.FormAnswerId,
                        principalTable: "FormAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormAnswerSelectedOptions_FormQuestionOptions_FormQuestionOptionId",
                        column: x => x.FormQuestionOptionId,
                        principalTable: "FormQuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormAnswers_QuestionId",
                table: "FormAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAnswers_ResponseId",
                table: "FormAnswers",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAnswerSelectedOptions_FormAnswerId",
                table: "FormAnswerSelectedOptions",
                column: "FormAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_FormAnswerSelectedOptions_FormQuestionOptionId",
                table: "FormAnswerSelectedOptions",
                column: "FormQuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormQuestionOptions_QuestionId",
                table: "FormQuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormQuestions_FormId",
                table: "FormQuestions",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormResponses_FormId",
                table: "FormResponses",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormResponses_RespondedByStudentId",
                table: "FormResponses",
                column: "RespondedByStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormResponses_RespondedByTeacherId",
                table: "FormResponses",
                column: "RespondedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_AccessToken",
                table: "Forms",
                column: "AccessToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CourseId",
                table: "Forms",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CreatedByManagerId",
                table: "Forms",
                column: "CreatedByManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CreatedByTeacherId",
                table: "Forms",
                column: "CreatedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_HalaqaId",
                table: "Forms",
                column: "HalaqaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormAnswerSelectedOptions");

            migrationBuilder.DropTable(
                name: "FormAnswers");

            migrationBuilder.DropTable(
                name: "FormQuestionOptions");

            migrationBuilder.DropTable(
                name: "FormResponses");

            migrationBuilder.DropTable(
                name: "FormQuestions");

            migrationBuilder.DropTable(
                name: "Forms");
        }
    }
}
