using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class MeasurementsAsValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "AspNetUsers",
                newName: "Measurments_Height");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AspNetUsers",
                newName: "DateOfWorkoutStart");

            migrationBuilder.AlterColumn<int>(
                name: "Measurments_Height",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Arms",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Chest",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Hips",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Shoulders",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Thights",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Waist",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Measurments_Weight",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measurments_Arms",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Measurments_Chest",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Measurments_Hips",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Measurments_Shoulders",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Measurments_Thights",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Measurments_Waist",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Measurments_Weight",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Measurments_Height",
                table: "AspNetUsers",
                newName: "Height");

            migrationBuilder.RenameColumn(
                name: "DateOfWorkoutStart",
                table: "AspNetUsers",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Arms = table.Column<decimal>(type: "TEXT", nullable: true),
                    Chest = table.Column<decimal>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hips = table.Column<decimal>(type: "TEXT", nullable: true),
                    Shoulders = table.Column<decimal>(type: "TEXT", nullable: true),
                    Thights = table.Column<decimal>(type: "TEXT", nullable: true),
                    Waist = table.Column<decimal>(type: "TEXT", nullable: true),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_UserId",
                table: "Measurements",
                column: "UserId");
        }
    }
}
