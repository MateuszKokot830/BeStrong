using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Settings_AutoPublishWorkoutPlanChanges",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Settings_AutoPublishWorkouts",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_MeasurementsVisibility",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_PhotosVisibility",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_WorkoutPlanVisibility",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_WorkoutsVisibility",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settings_AutoPublishWorkoutPlanChanges",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Settings_AutoPublishWorkouts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Settings_MeasurementsVisibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Settings_PhotosVisibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Settings_WorkoutPlanVisibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Settings_WorkoutsVisibility",
                table: "AspNetUsers");
        }
    }
}
