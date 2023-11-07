using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;

namespace Ms2dNapaj.Pages.Supplier
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly NapajDBContext _context;

        public IndexModel(NapajDBContext context)
        {
            _context = context;
        }

        public List<Ms2dNapaj.Models.Supplier> Suppliers { get; set; }

        public void OnGet()
        {
            try
            {
                Suppliers = _context.Suppliers.Include(s => s.SuppliedIngredients).ToList();
            }
            catch (Exception ex)
            {
                Suppliers = new List<Ms2dNapaj.Models.Supplier>(); // Initialisation d'une liste vide en cas d'�chec
                // Gestion des erreurs
            }
        }
    }
}
