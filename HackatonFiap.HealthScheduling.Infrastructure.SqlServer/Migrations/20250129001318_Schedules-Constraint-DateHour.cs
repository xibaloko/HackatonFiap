using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SchedulesConstraintDateHour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DateHour",
                table: "Schedules",
                column: "DateHour",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_DateHour",
                table: "Schedules");
        }
    }
}
