using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;
using System.Globalization;
using System.Text;

namespace Ms2dNapaj.Pages.Recette
{
    public class IndexModel : PageModel
    {
		private readonly NapajDBContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConverter _pdfConverter;

		public IndexModel(NapajDBContext context, IWebHostEnvironment webHostEnvironment, IConverter pdfConverter)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
			_pdfConverter = pdfConverter;
		}
		public List<Ms2dNapaj.Models.Recipe> Recipes { get; set; }
		public List<Ms2dNapaj.Models.Recipe> RecipesArchived { get; set; }
		public string ErrorMessage { get; set; }
		public void OnGet()
        {
			try
			{
				Recipes = _context.Recipes
					.Where(a => a.Status == RecipeStatus.Active)
					.Include(r => r.Ingredients)
						.ThenInclude(iq => iq.Ingredient)
							.ThenInclude(ii=>ii.Allergen)
					.Include(r => r.Allergens).ToList();

				RecipesArchived = _context.Recipes
					.Where(a=>a.Status == RecipeStatus.Archived)
					.Include(r => r.Ingredients)
						.ThenInclude(iq => iq.Ingredient)
					.Include(r => r.Allergens).ToList();
                //recherche des allergenes de la recette
             
                for (int i = 0; i < Recipes.Count; i++)
                {
                    for (int j = 0; j < Recipes[i].Ingredients.Count; j++)
                    {
                    Recipes[i].Allergens.Add(Recipes[i].Ingredients[j].Ingredient.Allergen);

					}
				}
            }
			catch (Exception ex)
			{
				Recipes = new List<Ms2dNapaj.Models.Recipe>(); // Initialisation d'une liste vide en cas d'échec
				ErrorMessage = "Une erreur s'est produite lors de la récupération des recettes. Veuillez réessayer.";
			}
		}


		public IActionResult OnGetGenerateCataloguePDF()
		{
			// Sélection des ingrédients dans la base de données
			Recipes = _context.Recipes
					.Include(r => r.Ingredients)
						.ThenInclude(iq => iq.Ingredient)
					.Include(r => r.Allergens).ToList();

			// Créer un document PDF
			var document = new HtmlToPdfDocument()
			{
				GlobalSettings = {
				PaperSize = PaperKind.A4,
				Orientation = Orientation.Portrait,
			},
				Objects = {
				new ObjectSettings() {
					HtmlContent = GenerateHtmlContentForPDF(Recipes),
				}
			}
			};

			// Convertir le document en tableau d'octets
			byte[] pdfBytes = _pdfConverter.Convert(document);

			// Utiliser le chemin racine de l'application fourni par le service IWebHostEnvironment
			var wwwrootPath = _webHostEnvironment.WebRootPath;
			var fileName = $"Catalogue_Produit_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
			var filePath = Path.Combine(wwwrootPath, "Catalogue_Produit", fileName);

			// Créer le dossier s'il n'existe pas
			Directory.CreateDirectory(Path.Combine(wwwrootPath, "Catalogue_Produit"));

			// Sauvegarder le fichier PDF
			System.IO.File.WriteAllBytes(filePath, pdfBytes);

			// Retourner le fichier PDF en téléchargement
			return File(pdfBytes, "application/pdf", fileName);
		}

		// Générer le contenu HTML pour le PDF
		private string GenerateHtmlContentForPDF(List<Ms2dNapaj.Models.Recipe> recp)
		{
			var htmlContent = new StringBuilder();

			htmlContent.AppendLine("<html><head>");
			htmlContent.AppendLine("<meta charset='utf-8'>");
			htmlContent.AppendLine("<style>");
			htmlContent.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 15px; }");
			htmlContent.AppendLine("table, th, td { border: 1px solid #dee2e6; }");
			htmlContent.AppendLine("th, td { padding: 10px; text-align: left; }");
			htmlContent.AppendLine("thead { background-color: #343a40; color: #fff; }");
			htmlContent.AppendLine("</style>");
			htmlContent.AppendLine("</head><body>");
			htmlContent.AppendLine("<div class='container mt-5'>");
			htmlContent.AppendLine("<h1 class='mb-4'>Catalogue produit de la patisserie NAPAJ</h1>");

			// Vérifie s'il y a des recettes à afficher
			if (recp.Any())
			{
				// Crée une table avec une classe personnalisée pour appliquer le style
				htmlContent.AppendLine("<div class='table-responsive'>");
				htmlContent.AppendLine("<table class='custom-table'>");

				// Ajoute l'en-tête de la table
				htmlContent.AppendLine("<thead>");
				htmlContent.AppendLine("<tr>");
				htmlContent.AppendLine("<th scope='col'>Recette</th>");
				htmlContent.AppendLine("<th scope='col'>Nombre de parts</th>");
				htmlContent.AppendLine("<th scope='col'>Prix</th>");
				htmlContent.AppendLine("</tr>");
				htmlContent.AppendLine("</thead>");

				// Ajoute les lignes de données
				htmlContent.AppendLine("<tbody>");
				foreach (var rep in recp)
				{
					htmlContent.AppendLine("<tr>");
					htmlContent.AppendLine($"<td>{rep.Name}</td>");
					htmlContent.AppendLine($"<td>{rep.NumberOfServings}</td>");
					htmlContent.AppendLine($"<td>{rep.SellingPrice.ToString("C2").Replace("€", "&euro;")}</td>");
					htmlContent.AppendLine("</tr>");
				}
				htmlContent.AppendLine("</tbody>");

				// Termine la table
				htmlContent.AppendLine("</table>");
				htmlContent.AppendLine("</div>");
			}
			else
			{
				htmlContent.AppendLine("<p class='mt-4'>Aucune recette à vendre pour le moment.</p>");
			}

			// Termine le document HTML
			htmlContent.AppendLine("</div></body></html>");

			return htmlContent.ToString();
		}

        public async Task<IActionResult> OnGetArchiveRecipeAsync(int id)
        {
            var recipeToArchive = await _context.Recipes.FindAsync(id);

            if (recipeToArchive != null)
            {
                // Archiver la recette (par exemple, en définissant un indicateur d'archivage)
                recipeToArchive.Status = RecipeStatus.Archived;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnGetDeArchiveRecipeAsync(int id)
        {
            var recipeToArchive = await _context.Recipes.FindAsync(id);

            if (recipeToArchive != null)
            {
                // Archiver la recette (par exemple, en définissant un indicateur d'archivage)
                recipeToArchive.Status = RecipeStatus.Active;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
