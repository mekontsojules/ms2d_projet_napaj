using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Ms2dNapaj.DAL;
using System.Configuration;

namespace Ms2dNapaj.Pages.Ingredient
{
    public class ImportModel : PageModel
    {
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly NapajDBContext _context;
        public ImportModel(IWebHostEnvironment hostingEnvironment, NapajDBContext context)
		{
			_hostingEnvironment = hostingEnvironment;
			_context = context;
		}

		public List<IFormFile> fichier { get; set; }
		public async Task<IActionResult> OnPostAsync(List<IFormFile> files)
		{
			var filesCount = Request.Form.Files.Count; // Vérifier combien de fichiers sont présents dans la requête

			foreach (var file in files)
			{
				if (file.Length > 0)
				{
					var uploadDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");

					// Créez le répertoire s'il n'existe pas
					if (!Directory.Exists(uploadDirectory))
					{
						Directory.CreateDirectory(uploadDirectory);
					}

					var filePath = Path.Combine(uploadDirectory, file.FileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					using (TextFieldParser parser = new TextFieldParser(filePath))
					{
						parser.HasFieldsEnclosedInQuotes = true;
						parser.SetDelimiters(",");

						// Lire les en-têtes
						string[] headers = parser.ReadFields();

						// Trouver les index des colonnes "Code", "Nom", et "Prix"
						int codeColumnIndex = Array.IndexOf(headers, "code");
						int nomColumnIndex = Array.IndexOf(headers, "nom");
						int priceColumnIndex = Array.IndexOf(headers, "prix");
						int fournisseurColumnIndex = Array.IndexOf(headers, "fournisseur");

						// Assurez-vous que les colonnes nécessaires existent dans le fichier
						if (codeColumnIndex == -1 || nomColumnIndex == -1 || priceColumnIndex == -1 || fournisseurColumnIndex==-1)
						{
							// Gérez le cas où les colonnes nécessaires ne sont pas présentes
							return RedirectToPage("./Index");
						}

						while (!parser.EndOfData)
						{
							// Lire les données ligne par ligne
							string[] fields = parser.ReadFields();

							var ingredientCode = fields[codeColumnIndex];
							var ingredientName = fields[nomColumnIndex];
							var priceString = fields[priceColumnIndex];
							var fournisseur = fields[fournisseurColumnIndex];

							// Assurez-vous que les valeurs nécessaires existent dans chaque ligne
							if (!string.IsNullOrEmpty(ingredientCode) && !string.IsNullOrEmpty(ingredientName) && decimal.TryParse(priceString, out var price) && int.TryParse(fournisseur,out int four))
							{
								// Votre logique pour mettre à jour ou ajouter l'ingrédient
								var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Code == ingredientCode);

								if (existingIngredient != null)
								{
									// Mettez à jour le prix de l'ingrédient existant
									existingIngredient.PurchasePrice = price;
								}
								else
								{
									// Ajoutez un nouvel ingrédient
									var newIngredient = new Models.Ingredient
									{
										Name = ingredientName,
										PurchasePrice = price,
										CurrentStock= 0,
										MinimumStock= 10,
										ImportDate= DateTime.Now,
										UnitedMesure="Kg",
										SupplierId= four,
										
										// Ajoutez d'autres propriétés de l'ingrédient si nécessaire
										Code = ingredientCode
									};

									_context.Ingredients.Add(newIngredient);
								}
							}
						}
					}

					await _context.SaveChangesAsync();
				}
			}

			// Gérez la redirection après l'importation
			return RedirectToPage("/Ingredient/Index");
		}


	}
}