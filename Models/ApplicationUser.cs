using Microsoft.AspNetCore.Identity;

namespace Ms2dNapaj.Models
{
    public class ApplicationUser: IdentityUser
    {

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string sexe { get; set; }
        
    }
}
