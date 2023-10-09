using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_rpg2.Migrations
{
    /// <inheritdoc />
    public partial class Victories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Victoris",
                table: "Characters",
                newName: "Victories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Victories",
                table: "Characters",
                newName: "Victoris");
        }
    }
}
