using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Allergene
{
    public class IndexModel : PageModel
    {
        private readonly Ms2dNapaj.DAL.NapajDBContext _context;

        public IndexModel(Ms2dNapaj.DAL.NapajDBContext context)
        {
            _context = context;
        }

		public List<Ms2dNapaj.Models.Allergen> Allergen { get; set; }
		public string ErrorMessage { get; set; }


		public  void OnGet()
        {
			try
			{
				Allergen = _context.Allergens.ToList() ;
			}
			catch (Exception ex)
			{
				Allergen = new List<Ms2dNapaj.Models.Allergen>(); // Initialisation d'une liste vide en cas d'échec
				ErrorMessage = "Une erreur s'est produite lors de la récupération des ingrédients. Veuillez réessayer.";
			}
		}
    }
}
