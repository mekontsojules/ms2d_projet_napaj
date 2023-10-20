namespace Ms2dNapaj.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient> SuppliedIngredients { get; set; }
        public List<SupplierIngredientRate> IngredientRates { get; set; }
    }
}
