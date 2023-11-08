using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;
using Microsoft.EntityFrameworkCore;

namespace Ms2dNapaj.Pages.Ingredient
{
    public class DeleteIngredientModel : PageModel
    {
        private readonly NapajDBContext _context;

        public DeleteIngredientModel(NapajDBContext context)
        {
            _context = context;
        }

        public Models.Ingredient Ingredient { get; set; }

        public IActionResult OnGet(int id)
        {
            Ingredient = _context.Ingredients.Include(i => i.Supplier).FirstOrDefault(i => i.Id == id);

            if (Ingredient == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Ingredient = await _context.Ingredients.FindAsync(id);

            if (Ingredient != null)
            {
                _context.Ingredients.Remove(Ingredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }

}
