using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RestructureWorkoutExerciseIntoSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reps",
                table: "WorkoutExercises");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "WorkoutExercises",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "Sets",
                table: "WorkoutExercises",
                newName: "Order");

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SetNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Reps = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: true),
                    WorkoutExerciseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_WorkoutExercises_WorkoutExerciseId",
                        column: x => x.WorkoutExerciseId,
                        principalTable: "WorkoutExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_WorkoutExerciseId",
                table: "WorkoutSets",
                column: "WorkoutExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "WorkoutExercises",
                newName: "Sets");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "WorkoutExercises",
                newName: "Weight");

            migrationBuilder.AddColumn<int>(
                name: "Reps",
                table: "WorkoutExercises",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
