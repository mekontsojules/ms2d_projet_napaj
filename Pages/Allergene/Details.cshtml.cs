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
    public class DetailsModel : PageModel
    {
        private readonly Ms2dNapaj.DAL.NapajDBContext _context;

        public DetailsModel(Ms2dNapaj.DAL.NapajDBContext context)
        {
            _context = context;
        }

      public Allergen Allergen { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Allergens == null)
            {
                return NotFound();
            }

            var allergen = await _context.Allergens.FirstOrDefaultAsync(m => m.Id == id);
            if (allergen == null)
            {
                return NotFound();
            }
            else 
            {
                Allergen = allergen;
            }
            return Page();
        }
    }
}
