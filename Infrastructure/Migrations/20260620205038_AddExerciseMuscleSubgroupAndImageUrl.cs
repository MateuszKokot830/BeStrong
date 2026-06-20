using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseMuscleSubgroupAndImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Exercises",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MuscleSubgroup",
                table: "Exercises",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "MuscleSubgroup",
                table: "Exercises");
        }
    }
}
