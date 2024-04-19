using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMvc.Migrations
{
    /// <inheritdoc />
    public partial class FinalDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Entry");
        }
    }
}
