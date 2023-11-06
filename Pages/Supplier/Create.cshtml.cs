using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;

namespace Ms2dNapaj.Pages.Supplier
{
    public class CreateModel : PageModel
    {
        private readonly NapajDBContext _context;

        public CreateModel(NapajDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Ms2dNapaj.Models.Supplier Supplier { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {

            _context.Suppliers.Add(Supplier);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
