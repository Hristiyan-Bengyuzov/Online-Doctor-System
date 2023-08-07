using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineDoctorSystem.Data.Migrations
{
    public partial class AddCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalendarEventId",
                table: "Consultations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_CalendarEventId",
                table: "Consultations",
                column: "CalendarEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_CalendarEvents_CalendarEventId",
                table: "Consultations",
                column: "CalendarEventId",
                principalTable: "CalendarEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_CalendarEvents_CalendarEventId",
                table: "Consultations");

            migrationBuilder.DropTable(
                name: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_Consultations_CalendarEventId",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "CalendarEventId",
                table: "Consultations");
        }
    }
}
