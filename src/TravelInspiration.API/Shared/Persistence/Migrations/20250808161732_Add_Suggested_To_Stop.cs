using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelInspiration.API.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Suggested_To_Stop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Suggested",
                table: "Stops",
                type: "bit",
                nullable: true,
                defaultValue: false);

            // Updating each record in the Stops table for the Suggested column
            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 1,
                columns: new string[] { "Suggested" },
                values: new object[] { false }
            );

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 2,
                columns: new string[] { "Suggested" },
                values: new object[] { false }
            );

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 3,
                columns: new string[] { "Suggested" },
                values: new object[] { false }
            );

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 4,
                columns: new string[] { "Suggested" },
                values: new object[] { false }
            );

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 5,
                columns: new string[] { "Suggested" },
                values: new object[] { false }
            );

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 6,
                columns: new string[] { "Suggested" },
                values: new object[] { false }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suggested",
                table: "Stops");
        }
    }
}
