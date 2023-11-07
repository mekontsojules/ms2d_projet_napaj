namespace Ms2dNapaj.Models
{
    public class SupplierIngredientRate
    {
        public int Id { get; set; }
        public int SupplierId { get; set; } // Clé étrangère pour le fournisseur
        public int IngredientId { get; set; } // Clé étrangère pour l'ingrédient
        public decimal Rate { get; set; }// 

        public virtual Supplier Supplier { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
