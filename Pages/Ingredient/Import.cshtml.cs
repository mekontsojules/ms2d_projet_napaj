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
			var filesCount = Request.Form.Files.Count; // V�rifier combien de fichiers sont pr�sents dans la requ�te

			foreach (var file in files)
			{
				if (file.Length > 0)
				{
					var uploadDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");

					// Cr�ez le r�pertoire s'il n'existe pas
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

						// Lire les en-t�tes
						string[] headers = parser.ReadFields();

						// Trouver les index des colonnes "Code", "Nom", et "Prix"
						int codeColumnIndex = Array.IndexOf(headers, "code");
						int nomColumnIndex = Array.IndexOf(headers, "nom");
						int priceColumnIndex = Array.IndexOf(headers, "prix");
						int fournisseurColumnIndex = Array.IndexOf(headers, "fournisseur");

						// Assurez-vous que les colonnes n�cessaires existent dans le fichier
						if (codeColumnIndex == -1 || nomColumnIndex == -1 || priceColumnIndex == -1 || fournisseurColumnIndex==-1)
						{
							// G�rez le cas o� les colonnes n�cessaires ne sont pas pr�sentes
							return RedirectToPage("./Index");
						}

						while (!parser.EndOfData)
						{
							// Lire les donn�es ligne par ligne
							string[] fields = parser.ReadFields();

							var ingredientCode = fields[codeColumnIndex];
							var ingredientName = fields[nomColumnIndex];
							var priceString = fields[priceColumnIndex];
							var fournisseur = fields[fournisseurColumnIndex];

							// Assurez-vous que les valeurs n�cessaires existent dans chaque ligne
							if (!string.IsNullOrEmpty(ingredientCode) && !string.IsNullOrEmpty(ingredientName) && decimal.TryParse(priceString, out var price) && int.TryParse(fournisseur,out int four))
							{
								// Votre logique pour mettre � jour ou ajouter l'ingr�dient
								var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Code == ingredientCode);

								if (existingIngredient != null)
								{
									// Mettez � jour le prix de l'ingr�dient existant
									existingIngredient.PurchasePrice = price;
								}
								else
								{
									// Ajoutez un nouvel ingr�dient
									var newIngredient = new Models.Ingredient
									{
										Name = ingredientName,
										PurchasePrice = price,
										CurrentStock= 0,
										MinimumStock= 10,
										ImportDate= DateTime.Now,
										UnitedMesure="Kg",
										SupplierId= four,
										
										// Ajoutez d'autres propri�t�s de l'ingr�dient si n�cessaire
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

			// G�rez la redirection apr�s l'importation
			return RedirectToPage("/Ingredient/Index");
		}


	}
}