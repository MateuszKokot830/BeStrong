using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class WorkingPlanInPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "Posts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_WorkoutPlanId",
                table: "Posts",
                column: "WorkoutPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_WorkoutPlans_WorkoutPlanId",
                table: "Posts",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_WorkoutPlans_WorkoutPlanId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_WorkoutPlanId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "Posts");
        }
    }
}
