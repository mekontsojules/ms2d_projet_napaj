using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ms2dNapaj.Migrations
{
    /// <inheritdoc />
    public partial class composante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "unite",
                table: "IngredientQuantities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unite",
                table: "IngredientQuantities");
        }
    }
}
