using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.DAL
{
    public class NapajDBContext : IdentityDbContext<ApplicationUser>
    {

        public NapajDBContext(DbContextOptions<NapajDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            base.OnModelCreating(builder);
        }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<IngredientQuantity> IngredientQuantities { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierIngredientRate> SupplierIngredientRates { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchasedIngredient> PurchasedIngredients { get; set; }
    }
}
