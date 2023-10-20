namespace Ms2dNapaj.Models
{
    public class PurchasedIngredient
    {
        public int Id { get; set; }
        public int IngredientId { get; set; } // Clé étrangère pour l'ingrédient
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
