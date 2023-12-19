namespace Ms2dNapaj.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public int SupplierId { get; set; }
        public DateTime ImportDate { get; set; } = DateTime.Now;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public int? AllergenId { get; set; }
        public string UnitedMesure { get;set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Allergen Allergen { get; set; }  

    }
}
