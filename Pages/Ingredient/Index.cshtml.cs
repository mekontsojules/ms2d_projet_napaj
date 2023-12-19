using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ms2dNapaj.DAL;
using Ms2dNapaj.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace Ms2dNapaj.Pages.Ingredient
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly NapajDBContext _context;
		private readonly IConverter _pdfConverter;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public IndexModel(NapajDBContext context, IWebHostEnvironment webHostEnvironment, IConverter pdfConverter)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
			_pdfConverter = pdfConverter;
		}

		public List<Ms2dNapaj.Models.Ingredient> Ingredients { get; set; }
		public string ErrorMessage { get; set; }

		public void OnGet()
		{
			try
			{
				Ingredients = _context.Ingredients.Include(i=>i.Supplier).Include(a=>a.Allergen).ToList();
			}
			catch (Exception ex)
			{

				Ingredients = new List<Ms2dNapaj.Models.Ingredient>(); // Initialisation d'une liste vide en cas d'échec
				ErrorMessage = "Une erreur s'est produite lors de la récupération des ingrédients. Veuillez réessayer.";

			}
		}


		public IActionResult OnGetGenerateShoppingList()
		{
			// Sélection des ingrédients dans la base de données
			var ingredientsToBuy = _context.Ingredients
				.Where(i => i.CurrentStock < i.MinimumStock)
				.ToList();

			// Créer un nouveau fichier Excel
			using (ExcelPackage package = new ExcelPackage())
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ShoppingList");

				// Ajouter le titre
				worksheet.Cells["A1:B1"].Merge = true;
				worksheet.Cells[1, 1].Value = "Liste d'achat d'ingrédients pour la pâtisserie NAPAJ";
				worksheet.Cells[1, 1].Style.Font.Bold = true;
				worksheet.Cells[1, 1].Style.Font.Size = 16;
				worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

				// Ajouter les en-têtes
				worksheet.Cells[2, 1].Value = "Nom de l'ingrédient";
				worksheet.Cells[2, 2].Value = "Quantité à acheter";

				// Remplir les données
				for (int i = 0; i < ingredientsToBuy.Count; i++)
				{
					worksheet.Cells[i + 3, 1].Value = ingredientsToBuy[i].Name;
					worksheet.Cells[i + 3, 2].Value = ingredientsToBuy[i].MinimumStock * 3;
				}

				// Ajuster automatiquement la largeur des colonnes
				worksheet.Cells.AutoFitColumns();

				// Convertir le fichier Excel en tableau d'octets
				byte[] excelBytes = package.GetAsByteArray();

				// Utiliser le chemin racine de l'application fourni par le service IWebHostEnvironment
				var wwwrootPath = _webHostEnvironment.WebRootPath;
				var fileName = $"list_achat_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
				var filePath = Path.Combine(wwwrootPath, "shopping_lists", fileName);

				// Créer le dossier s'il n'existe pas
				Directory.CreateDirectory(Path.Combine(wwwrootPath, "shopping_lists"));

				// Sauvegarder le fichier
				System.IO.File.WriteAllBytes(filePath, excelBytes);

				// Retourner le fichier Excel en téléchargement
				return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
			}
		}


		public IActionResult OnGetGenerateShoppingListPDF()
		{
			// Sélection des ingrédients dans la base de données
			var ingredientsToBuy = _context.Ingredients
				.Where(i => i.CurrentStock < i.MinimumStock)
				.ToList();

			// Créer un document PDF
			var document = new HtmlToPdfDocument()
			{
				GlobalSettings = {
				PaperSize = PaperKind.A4,
				Orientation = Orientation.Portrait,
			},
				Objects = {
				new ObjectSettings() {
					HtmlContent = GenerateHtmlContentForPDF(ingredientsToBuy),
				}
			}
			};

			// Convertir le document en tableau d'octets
			byte[] pdfBytes = _pdfConverter.Convert(document);

			// Utiliser le chemin racine de l'application fourni par le service IWebHostEnvironment
			var wwwrootPath = _webHostEnvironment.WebRootPath;
			var fileName = $"list_achat_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
			var filePath = Path.Combine(wwwrootPath, "shopping_lists", fileName);

			// Créer le dossier s'il n'existe pas
			Directory.CreateDirectory(Path.Combine(wwwrootPath, "shopping_lists"));

			// Sauvegarder le fichier PDF
			System.IO.File.WriteAllBytes(filePath, pdfBytes);

			// Retourner le fichier PDF en téléchargement
			return File(pdfBytes, "application/pdf", fileName);
		}

		// Générer le contenu HTML pour le PDF
		private string GenerateHtmlContentForPDF(List<Ms2dNapaj.Models.Ingredient> ingredients)
		{
			var htmlContent = new StringBuilder();

			htmlContent.AppendLine("<html><body>");
			htmlContent.AppendLine("<h1>Liste d'achat d'ingredients pour la patisserie NAPAJ</h1>");

			// Vérifie s'il y a des ingrédients à acheter
			if (ingredients.Any())
			{
				// Crée une table HTML
				htmlContent.AppendLine("<table border='1'>");

				// Ajoute l'en-tête de la table
				htmlContent.AppendLine("<tr>");
				htmlContent.AppendLine("<th>Nom de l'ingredient</th>");
				htmlContent.AppendLine("<th>Quantite a acheter</th>");
				htmlContent.AppendLine("</tr>");

				// Ajoute les lignes de données
				foreach (var ingredient in ingredients)
				{
					htmlContent.AppendLine("<tr>");
					htmlContent.AppendLine($"<td>{ingredient.Name}</td>");
					htmlContent.AppendLine($"<td>{ingredient.MinimumStock * 3}</td>");
					htmlContent.AppendLine("</tr>");
				}

				// Termine la table HTML
				htmlContent.AppendLine("</table>");
			}
			else
			{
				htmlContent.AppendLine("<p>Aucun ingrédient à acheter pour le moment.</p>");
			}

			// Termine le document HTML
			htmlContent.AppendLine("</body></html>");

			return htmlContent.ToString();
		}



	}

}
