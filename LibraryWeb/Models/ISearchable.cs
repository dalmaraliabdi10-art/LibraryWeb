namespace LibraryWeb.Models
{
    public interface ISearchable
    {
        bool Matches(string searchTerm);
    }
}