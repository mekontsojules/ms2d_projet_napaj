namespace Ms2dNapaj.Models
{
    public class ProductCatalog
    {
        public int Id { get; set; }
        public List<Recipe> Recipes { get; set; }
        public DateTime GenerationDate { get; set; } = DateTime.Now;
        public string PdfFileUrl { get; set; }
        //presenter les ingredeint et les prixs
    }
}
