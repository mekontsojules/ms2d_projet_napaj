using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ms2dNapaj.Migrations
{
    /// <inheritdoc />
    public partial class codeingredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Ingredients");
        }
    }
}
