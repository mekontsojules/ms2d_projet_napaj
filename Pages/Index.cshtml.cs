using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly NapajDBContext _dbContext;
        public IndexModel(ILogger<IndexModel> logger,NapajDBContext napajDBContext)
        {
            _logger = logger;
            _dbContext = napajDBContext;
        }

        [BindProperty]
        public List<Models.Ingredient> ingredients { get; set; } = new List<Models.Ingredient>();

        [BindProperty]
        public int NombreIngredient { get; set; }
        [BindProperty]
        public int NombreRecette { get; set; }
        [BindProperty]
        public int nombreAllergen { get; set; }
        [BindProperty]
        public string moyenne { get; set; }
        public void OnGet()
        {
            ingredients=_dbContext.Ingredients.ToList();
            NombreIngredient = _dbContext.Ingredients.Count();
            NombreRecette = _dbContext.Recipes.Count();
            nombreAllergen = _dbContext.Allergens.Count();
            moyenne=_dbContext.Recipes.Average(r=>r.SellingPrice).ToString();

        }
    }
}