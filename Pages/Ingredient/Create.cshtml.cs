using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Ingredient
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly NapajDBContext _context;

        public CreateModel(NapajDBContext context)
        {
            _context = context;
        }

        public List<Ms2dNapaj.Models.Supplier> Suppliers { get; set; }
        public List<Ms2dNapaj.Models.Allergen> Allergenes { get; set; }

        [BindProperty]
        public Models.Ingredient Ingredient { get; set; }

        public IActionResult OnGet()
        {
            Suppliers = _context.Suppliers.ToList();
            Allergenes=_context.Allergens.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Ingredient.ImportDate = DateTime.Now;

            _context.Ingredients.Add(Ingredient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
