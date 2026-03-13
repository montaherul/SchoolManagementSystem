using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_management_system.Migrations
{
    /// <inheritdoc />
    public partial class result : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PassMarks",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalMarks",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "GPA",
                table: "Results",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Results",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Results",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Results",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPassed",
                table: "Marks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ExamRoutines",
                columns: table => new
                {
                    ExamRoutineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvigilatorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamRoutines", x => x.ExamRoutineID);
                    table.ForeignKey(
                        name: "FK_ExamRoutines_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamRoutines_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamRoutines_Teachers_InvigilatorID",
                        column: x => x.InvigilatorID,
                        principalTable: "Teachers",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamRoutines_ExamID",
                table: "ExamRoutines",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRoutines_InvigilatorID",
                table: "ExamRoutines",
                column: "InvigilatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamRoutines_SubjectID",
                table: "ExamRoutines",
                column: "SubjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamRoutines");

            migrationBuilder.DropColumn(
                name: "PassMarks",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TotalMarks",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "IsPassed",
                table: "Marks");
        }
    }
}
