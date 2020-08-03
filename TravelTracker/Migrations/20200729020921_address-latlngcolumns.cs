using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelTracker.Migrations
{
    public partial class addresslatlngcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Destinations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Destinations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LatLngId",
                table: "Destinations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LatLng",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<double>(nullable: false),
                    lng = table.Column<double>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Address",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "LatLngId",
                table: "Destinations");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
