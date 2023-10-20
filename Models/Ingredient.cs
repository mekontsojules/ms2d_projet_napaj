namespace Ms2dNapaj.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Supplier { get; set; }
        public DateTime ImportDate { get; set; } = DateTime.Now;
        public decimal CurrentStock { get; set; }
        public string Allergens { get; set; }
    }
}
