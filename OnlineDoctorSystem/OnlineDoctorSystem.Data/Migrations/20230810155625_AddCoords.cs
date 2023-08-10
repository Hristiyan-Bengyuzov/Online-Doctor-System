using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class AddCoords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Doctors");
        }
    }
}
