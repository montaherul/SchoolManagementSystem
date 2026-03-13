using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_management_system.Migrations
{
    public partial class AddRollUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_AdmissionYear_ClassID_RollNumber",
                table: "Students",
                columns: new[] { "AdmissionYear", "ClassID", "RollNumber" },
                unique: true,
                filter: "[RollNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_AdmissionYear_ClassID_RollNumber",
                table: "Students");
        }
    }
}
