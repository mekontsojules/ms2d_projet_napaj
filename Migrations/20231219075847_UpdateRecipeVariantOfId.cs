using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ms2dNapaj.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeVariantOfId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergens",
                table: "Ingredients");

            migrationBuilder.AlterColumn<int>(
                name: "VariantOfId",
                table: "Recipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AllergenId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_AllergenId",
                table: "Ingredients",
                column: "AllergenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Allergens_AllergenId",
                table: "Ingredients",
                column: "AllergenId",
                principalTable: "Allergens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Allergens_AllergenId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_AllergenId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "AllergenId",
                table: "Ingredients");

            migrationBuilder.AlterColumn<int>(
                name: "VariantOfId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergens",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
