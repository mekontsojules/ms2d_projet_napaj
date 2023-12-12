using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Ingredient
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly NapajDBContext _context;

		public IndexModel(NapajDBContext context)
		{
			_context = context;
		}

		public List<Ms2dNapaj.Models.Ingredient> Ingredients { get; set; }
		public string ErrorMessage { get; set; }

		public void OnGet()
		{
			try
			{
				Ingredients = _context.Ingredients.Include(i=>i.Supplier).ToList();
			}
			catch (Exception ex)
			{

				Ingredients = new List<Ms2dNapaj.Models.Ingredient>(); // Initialisation d'une liste vide en cas d'échec
				ErrorMessage = "Une erreur s'est produite lors de la récupération des ingrédients. Veuillez réessayer.";

			}
		}
	}
}
