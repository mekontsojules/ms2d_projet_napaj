using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Allergene
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
        public Allergen Allergen { get; set; } 

        public IActionResult OnGet()
        {
			Suppliers = _context.Suppliers.ToList();
			return Page();
			
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            _context.Allergens.Add(Allergen);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

		/*  public void  OnPost()
	{

		_context.Allergens.Add(Allergen);
		 _context.SaveChanges();

	  //  return RedirectToPage("./Index");
	}*/
	}
}
