using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class MeasurementsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Measurments_Weight",
                table: "AspNetUsers",
                newName: "Measurements_Weight");

            migrationBuilder.RenameColumn(
                name: "Measurments_Waist",
                table: "AspNetUsers",
                newName: "Measurements_Waist");

            migrationBuilder.RenameColumn(
                name: "Measurments_Thights",
                table: "AspNetUsers",
                newName: "Measurements_Thights");

            migrationBuilder.RenameColumn(
                name: "Measurments_Shoulders",
                table: "AspNetUsers",
                newName: "Measurements_Shoulders");

            migrationBuilder.RenameColumn(
                name: "Measurments_Hips",
                table: "AspNetUsers",
                newName: "Measurements_Hips");

            migrationBuilder.RenameColumn(
                name: "Measurments_Height",
                table: "AspNetUsers",
                newName: "Measurements_Height");

            migrationBuilder.RenameColumn(
                name: "Measurments_Chest",
                table: "AspNetUsers",
                newName: "Measurements_Chest");

            migrationBuilder.RenameColumn(
                name: "Measurments_Arms",
                table: "AspNetUsers",
                newName: "Measurements_Arms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Measurements_Weight",
                table: "AspNetUsers",
                newName: "Measurments_Weight");

            migrationBuilder.RenameColumn(
                name: "Measurements_Waist",
                table: "AspNetUsers",
                newName: "Measurments_Waist");

            migrationBuilder.RenameColumn(
                name: "Measurements_Thights",
                table: "AspNetUsers",
                newName: "Measurments_Thights");

            migrationBuilder.RenameColumn(
                name: "Measurements_Shoulders",
                table: "AspNetUsers",
                newName: "Measurments_Shoulders");

            migrationBuilder.RenameColumn(
                name: "Measurements_Hips",
                table: "AspNetUsers",
                newName: "Measurments_Hips");

            migrationBuilder.RenameColumn(
                name: "Measurements_Height",
                table: "AspNetUsers",
                newName: "Measurments_Height");

            migrationBuilder.RenameColumn(
                name: "Measurements_Chest",
                table: "AspNetUsers",
                newName: "Measurments_Chest");

            migrationBuilder.RenameColumn(
                name: "Measurements_Arms",
                table: "AspNetUsers",
                newName: "Measurments_Arms");
        }
    }
}
