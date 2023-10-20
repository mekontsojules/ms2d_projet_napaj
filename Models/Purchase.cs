using Microsoft.AspNetCore.Identity;

namespace Ms2dNapaj.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public List<PurchasedIngredient> PurchasedIngredients { get; set; }
        public int SupplierId { get; set; } // Clé étrangère pour le fournisseur
        public string UserId { get; set; } // Clé étrangère pour l'utilisateur

        public virtual Supplier Supplier { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
