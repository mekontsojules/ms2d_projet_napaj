using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;

namespace Ms2dNapaj.Pages.Recette
{
    public class CreateModel : PageModel
    {
		private readonly NapajDBContext _context;

		public CreateModel(NapajDBContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
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
