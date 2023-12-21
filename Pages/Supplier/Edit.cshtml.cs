using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ms2dNapaj.DAL;

namespace Ms2dNapaj.Pages.Supplier
{
    public class EditModel : PageModel
    {
        public readonly NapajDBContext NapajDBContext;
        public EditModel(NapajDBContext _napajDBContext)
        {
            NapajDBContext = _napajDBContext;
        }
        [BindProperty]
        public Models.Supplier fournisseur { get; set; }
        public void OnGet(int id)
        {
            fournisseur = NapajDBContext.Suppliers.Where(i => i.Id == id).FirstOrDefault();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var existting = NapajDBContext.Suppliers.Where(i => i.Id == fournisseur.Id).FirstOrDefault();
                existting.Name = fournisseur.Name;

                NapajDBContext.Suppliers.Update(existting);
                NapajDBContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return RedirectToPage("./Index");
        }
    }
}
