using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;

namespace Ms2dNapaj.Pages.Recette
{
    public class IndexModel : PageModel
    {
		private readonly NapajDBContext _context;

		public IndexModel(NapajDBContext context)
		{
			_context = context;
		}
		public List<Ms2dNapaj.Models.Recipe> Recipes { get; set; }
		public string ErrorMessage { get; set; }
		public void OnGet()
        {
			try
			{
				Recipes = _context.Recipes.Include(r => r.Ingredients).Include(r => r.Allergens).ToList();
			}
			catch (Exception ex)
			{
				Recipes = new List<Ms2dNapaj.Models.Recipe>(); // Initialisation d'une liste vide en cas d'échec
				ErrorMessage = "Une erreur s'est produite lors de la récupération des recettes. Veuillez réessayer.";
			}
		}
    }
}
