namespace Ms2dNapaj.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IngredientQuantity> Ingredients { get; set; }
        public int NumberOfServings { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPricePerKg { get; set; }
        public DateTime CreationDate { get; set; }=DateTime.Now;
        public List<Allergen> Allergens { get; set; }
        public RecipeStatus Status { get; set; }
        public Recipe VariantOf { get; set; }
    }
}
