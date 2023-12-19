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
    public class DeleteModel : PageModel
    {
        private readonly Ms2dNapaj.DAL.NapajDBContext _context;

        public DeleteModel(Ms2dNapaj.DAL.NapajDBContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Allergen Allergen { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
          

            var allergen =  _context.Allergens.FirstOrDefaultAsync(m => m.Id == id);

            if (allergen == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Allergens == null)
            {
                return NotFound();
            }
            var allergen = await _context.Allergens.FindAsync(id);

            if (allergen != null)
            {
                Allergen = allergen;
                _context.Allergens.Remove(Allergen);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");


    
        }
    }
}
