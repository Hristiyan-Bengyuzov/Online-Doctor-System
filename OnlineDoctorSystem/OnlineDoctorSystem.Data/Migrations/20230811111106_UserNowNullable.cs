using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class UserNowNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_DoctorUserId",
                table: "Doctors");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorUserId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DoctorUserId",
                table: "Doctors",
                column: "DoctorUserId",
                unique: true,
                filter: "[DoctorUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_DoctorUserId",
                table: "Doctors");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorUserId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DoctorUserId",
                table: "Doctors",
                column: "DoctorUserId",
                unique: true);
        }
    }
}
