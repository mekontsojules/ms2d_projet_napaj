using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Recette
{
    public class EditRecipeModel : PageModel
    {
		private readonly NapajDBContext _context;

        public EditRecipeModel(NapajDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.Recipe Recipe { get; set; }

        [BindProperty]
        public List<IngredientQuantity> SelectedIngredients { get; set; } = new List<IngredientQuantity>();

        public List<Models.Ingredient> ListIngredients { get; set; } = new List<Models.Ingredient>();

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = _context.Recipes.Find(id);

            if (Recipe == null)
            {
                return NotFound();
            }

            ListIngredients = _context.Ingredients.ToList();
            ViewData["ListIngredient"] = new SelectList(ListIngredients, "Id", "Name");

            // Fetch the existing ingredient quantities for this recipe
            SelectedIngredients = _context.IngredientQuantities
                .Where(iq => iq.RecipeId == id).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the existing ingredient quantities for this recipe
            var existingIngredientQuantities = _context.IngredientQuantities
                .Where(iq => iq.RecipeId == id)
                .ToList();

            // Remove existing ingredient quantities
            //_context.IngredientQuantities.RemoveRange(existingIngredientQuantities);

            // Add the updated and new ingredient quantities
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                var ingredientIdString = Request.Form[$"SelectedIngredients[{i}].IngredientId"];
                var quantityString = Request.Form[$"SelectedIngredients[{i}].Quantity"];

                // Check if the values are not null or empty before parsing
                if (!string.IsNullOrEmpty(ingredientIdString) && int.TryParse(ingredientIdString, out var ingredientId)
                    && !string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out var quantity))
                {
                    IngredientQuantity newIngredientQuantity = new IngredientQuantity
                    {
                        IngredientId = ingredientId,
                        RecipeId = (int)id,
                        Quantity = quantity
                    };

                    _context.IngredientQuantities.Add(newIngredientQuantity);
                }
            }

            // 1. Calcul du coût de revient
            decimal costPrice = CalculateCostPrice(); // Implémentez cette fonction pour calculer le coût de revient

            // 2. Calcul du prix de vente (avec une marge de 70%)
            decimal sellingPrice = costPrice * (decimal)4.0; // 75% de marge, ajustez selon vos besoins

            // 3. Affectation des valeurs calculées à la recette
            Recipe.CostPricePerKg = costPrice;
            Recipe.SellingPrice = sellingPrice;
            Recipe.CreationDate = DateTime.Now;

            // Update other properties of the recipe
            var existingRecipe = _context.Recipes.Find(id);
            existingRecipe.Name = Recipe.Name;
            existingRecipe.NumberOfServings = Recipe.NumberOfServings;
            // Update other properties as needed

            await _context.SaveChangesAsync();

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
                totalCost += (ingredientData.PurchasePrice / 1000) * ingredient.Quantity;
            }

            return totalCost;
        }


    }
}