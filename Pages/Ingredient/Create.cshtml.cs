using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Ingredient
{
    public class CreateModel : PageModel
    {
        private readonly NapajDBContext _context;

        public CreateModel(NapajDBContext context)
        {
            _context = context;
        }

        public List<Ms2dNapaj.Models.Supplier> Suppliers { get; set; }

        [BindProperty]
        public Models.Ingredient Ingredient { get; set; }

        public IActionResult OnGet()
        {
            Suppliers = _context.Suppliers.ToList();
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
