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
        public List<int> SelectedIngredients { get; set; }

        public List<Models.Ingredient> ListIngredients { get; set; }=new List<Models.Ingredient>();
		public List<Models.Recipe> Listrecettes { get; set; }=new List<Models.Recipe>();

        public IActionResult OnGet()
		{
			ListIngredients = _context.Ingredients.ToList();
            ViewData["Listrecettes"] = new SelectList(_context.Recipes, "Id", "Name");
            return Page();
		}

		[BindProperty]
		public Models.Recipe Recipe { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{

			_context.Recipes.Add(Recipe);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	
	}
}
