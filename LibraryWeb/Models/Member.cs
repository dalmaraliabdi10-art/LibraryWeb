using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models
{
    public class Member
    {
        public int Id { get; set; } // Primärnyckel
        [Required]
        public string Name { get; set; } = ""; // Medlemmens namn
        [Required]
        public string Email { get; set; } = ""; // Medlemmens e-postadress
        [Required]
        public string Password { get; set; } = ""; // Lösenord
        public bool IsAdmin { get; set; } = false; // Flagga för att indikera om medlemmen är en administratör
    }
}