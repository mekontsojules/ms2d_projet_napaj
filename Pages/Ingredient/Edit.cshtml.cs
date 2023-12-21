using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;

namespace Ms2dNapaj.Pages.Ingredient
{
    public class EditModel : PageModel
    {

        public readonly NapajDBContext NapajDBContext;
        public EditModel(NapajDBContext _napajDBContext) 
        {
            NapajDBContext = _napajDBContext;
        }
        public List<Ms2dNapaj.Models.Supplier> Suppliers { get; set; }
        public List<Ms2dNapaj.Models.Allergen> Allergenes { get; set; }
        [BindProperty] 
        public Models.Ingredient Ingredient { get; set; }
        public void OnGet(int id)
        {
            Suppliers = NapajDBContext.Suppliers.ToList();
            Allergenes = NapajDBContext.Allergens.ToList();
            Ingredient = NapajDBContext.Ingredients.Where(i => i.Id == id).Include(s => s.Supplier).Include(a => a.Allergen).FirstOrDefault();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var existting=NapajDBContext.Ingredients.Where(i => i.Id == Ingredient.Id).Include(s=>s.Supplier).Include(a=>a.Allergen).FirstOrDefault();   
                existting.SupplierId=Ingredient.SupplierId;
                existting.PurchasePrice=Ingredient.PurchasePrice;
                existting.CurrentStock=Ingredient.CurrentStock;
                existting.AllergenId=Ingredient.AllergenId;
                existting.Code=Ingredient.Code;
                existting.Name=Ingredient.Name;

                NapajDBContext.Ingredients.Update(existting);
                NapajDBContext.SaveChanges();
            }catch(Exception ex)
            {

            }
            return RedirectToPage("./Index");
        }
    }
}
