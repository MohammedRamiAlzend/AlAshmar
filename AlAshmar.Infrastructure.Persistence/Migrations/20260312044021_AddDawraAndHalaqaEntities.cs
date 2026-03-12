using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlAshmar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAndHalaqaEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassEventsPoints_Semesters_SmesterId",
                table: "StudentClassEventsPoints");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SemesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Halaqas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halaqas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Halaqas_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuraanPages_ClassId",
                table: "StudentQuraanPages",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentHadiths_ClassId",
                table: "StudentHadiths",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassEventsPoints_ClassId",
                table: "StudentClassEventsPoints",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassEventsPoints_EventId",
                table: "StudentClassEventsPoints",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ClassId",
                table: "Points",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_EventId",
                table: "Points",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTeacherEnrollments_ClassId",
                table: "ClassTeacherEnrollments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentEnrollments_ClassId",
                table: "ClassStudentEnrollments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Halaqas_CourseId",
                table: "Halaqas",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudentEnrollments_Halaqas_ClassId",
                table: "ClassStudentEnrollments",
                column: "ClassId",
                principalTable: "Halaqas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassTeacherEnrollments_Halaqas_ClassId",
                table: "ClassTeacherEnrollments",
                column: "ClassId",
                principalTable: "Halaqas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Courses_EventId",
                table: "Points",
                column: "EventId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Halaqas_ClassId",
                table: "Points",
                column: "ClassId",
                principalTable: "Halaqas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassEventsPoints_Courses_EventId",
                table: "StudentClassEventsPoints",
                column: "EventId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassEventsPoints_Halaqas_ClassId",
                table: "StudentClassEventsPoints",
                column: "ClassId",
                principalTable: "Halaqas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassEventsPoints_Semesters_SmesterId",
                table: "StudentClassEventsPoints",
                column: "SmesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentHadiths_Halaqas_ClassId",
                table: "StudentHadiths",
                column: "ClassId",
                principalTable: "Halaqas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentQuraanPages_Halaqas_ClassId",
                table: "StudentQuraanPages",
                column: "ClassId",
                principalTable: "Halaqas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudentEnrollments_Halaqas_ClassId",
                table: "ClassStudentEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassTeacherEnrollments_Halaqas_ClassId",
                table: "ClassTeacherEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Courses_EventId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Halaqas_ClassId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassEventsPoints_Courses_EventId",
                table: "StudentClassEventsPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassEventsPoints_Halaqas_ClassId",
                table: "StudentClassEventsPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassEventsPoints_Semesters_SmesterId",
                table: "StudentClassEventsPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentHadiths_Halaqas_ClassId",
                table: "StudentHadiths");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentQuraanPages_Halaqas_ClassId",
                table: "StudentQuraanPages");

            migrationBuilder.DropTable(
                name: "Halaqas");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_StudentQuraanPages_ClassId",
                table: "StudentQuraanPages");

            migrationBuilder.DropIndex(
                name: "IX_StudentHadiths_ClassId",
                table: "StudentHadiths");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassEventsPoints_ClassId",
                table: "StudentClassEventsPoints");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassEventsPoints_EventId",
                table: "StudentClassEventsPoints");

            migrationBuilder.DropIndex(
                name: "IX_Points_ClassId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_EventId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_ClassTeacherEnrollments_ClassId",
                table: "ClassTeacherEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_ClassStudentEnrollments_ClassId",
                table: "ClassStudentEnrollments");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassEventsPoints_Semesters_SmesterId",
                table: "StudentClassEventsPoints",
                column: "SmesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
