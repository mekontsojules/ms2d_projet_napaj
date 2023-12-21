using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Recette
{
    public class CreateModel : PageModel
    {
		private readonly NapajDBContext _context;

		public CreateModel(NapajDBContext context)
		{
			_context = context;
		}

        [BindProperty]
        public List<IngredientModel> SelectedIngredients { get; set; } = new List<IngredientModel>();

        public List<IngredientQuantity> IngredientQuantité { get; set; }

        public List<Models.Ingredient> ListIngredients { get; set; }=new List<Models.Ingredient>();
		public List<Models.Recipe> Listrecettes { get; set; }=new List<Models.Recipe>();

        public IActionResult OnGet()
		{
			ListIngredients = _context.Ingredients.ToList();
            ViewData["Listrecettes"] = new SelectList(_context.Recipes, "Id", "Name");
            ViewData["ListIngredient"] = new SelectList(_context.Ingredients, "Id", "Name");
            return Page();
		}

		[BindProperty]
		public Models.Recipe Recipe { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Calcul du coût de revient
            decimal costPrice = CalculateCostPrice(); // Implémentez cette fonction pour calculer le coût de revient

            // 2. Calcul du prix de vente (avec une marge de 70%)
            decimal sellingPrice = costPrice * (decimal)4.0; // 75% de marge, ajustez selon vos besoins

            // 3. Affectation des valeurs calculées à la recette
            Recipe.CostPricePerKg = costPrice;
            Recipe.SellingPrice = sellingPrice;
            Recipe.CreationDate = DateTime.Now;

            // 4. Ajout de la recette à la base de données
            _context.Recipes.Add(Recipe);
            await _context.SaveChangesAsync();

            // 5. Récupération de l'ID de la recette créée
            int recipeId = Recipe.Id;

            // 6. Ajout des ingrédients à la table IngredientQuantity
            foreach (var ingredient in SelectedIngredients)
            {
                IngredientQuantity ingredientQuantity = new IngredientQuantity
                {
                    IngredientId = ingredient.IngredientId,
                    RecipeId = recipeId,
                    Quantity = ingredient.Quantity
                };

                _context.IngredientQuantities.Add(ingredientQuantity);
            }

            // 7. Sauvegarde des changements dans la base de données
            await _context.SaveChangesAsync();

            // Redirection vers la page d'index ou une autre page
            return RedirectToPage("./Index");
        }

        private decimal CalculateCostPrice()
        {
            // Implémentez la logique pour calculer le coût de revient en fonction des ingrédients sélectionnés
            decimal totalCost = 0;

            foreach (var ingredient in SelectedIngredients)
            {
                // Récupérez le prix au kilogramme de chaque ingrédient
                var ingredientData = _context.Ingredients.Find(ingredient.IngredientId);

                // Ajoutez le coût de cet ingrédient à la somme totale
                totalCost += (ingredientData.PurchasePrice/1000) * ingredient.Quantity;
            }

            return totalCost;
        }


    }
}
