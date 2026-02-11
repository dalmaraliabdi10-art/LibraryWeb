using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models
{
    public class Book : ISearchable
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string ISBN { get; set; } = "";
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; } = true;

        public bool Matches(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return false;
            string term = searchTerm.ToLower();
            return Title.ToLower().Contains(term) || 
                   Author.ToLower().Contains(term) || 
                   ISBN.Contains(term);
        }
    }
}