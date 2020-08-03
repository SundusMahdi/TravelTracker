using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelTracker.Migrations
{
    public partial class LatLngIsTwoColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_LatLng_LatLngId",
                table: "Destinations");

            migrationBuilder.DropTable(
                name: "LatLng");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_LatLngId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "LatLngId",
                table: "Destinations");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "Destinations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Destinations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "Destinations",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Destinations");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LatLngId",
                table: "Destinations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LatLng",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    lng = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LatLng", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_LatLngId",
                table: "Destinations",
                column: "LatLngId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_LatLng_LatLngId",
                table: "Destinations",
                column: "LatLngId",
                principalTable: "LatLng",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
