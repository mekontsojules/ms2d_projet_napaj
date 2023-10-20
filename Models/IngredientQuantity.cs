namespace Ms2dNapaj.Models
{
    public class IngredientQuantity
    {
        public int Id { get; set; }
        public int IngredientId { get; set; } // Clé étrangère pour l'ingrédient
        public int RecipeId { get; set; }    // Clé étrangère pour la recette
        public decimal Quantity { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
