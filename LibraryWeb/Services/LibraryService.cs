using Microsoft.EntityFrameworkCore;
using LibraryWeb.Data;
using LibraryWeb.Models;

namespace LibraryWeb.Services
{
    public class LibraryService
    {
        // Vi använder IDbContextFactory för att skapa nya DbContext-instans varje gång vi behöver interagera med databasen.
        // Detta är viktigt i Blazor Server-applikationer för att undvika problem med delade DbContext-instans över flera användare.
        private readonly IDbContextFactory<LibraryContext> _factory;

        public LibraryService(IDbContextFactory<LibraryContext> factory)
        {
            _factory = factory;
        }

        // Hämta alla böcker
        public async Task<List<Book>> GetBooksAsync()
        {
            // 
            using var context = _factory.CreateDbContext();
            return await context.Books.ToListAsync();
        }

        // Lägga till en ny bok (endast admin)
        public async Task AddBookAsync(Book book)
        {
            using var context = _factory.CreateDbContext();
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }

        // Registrera en ny medlem
        public async Task RegisterMemberAsync(Member member)
        {
            using var context = _factory.CreateDbContext();
            
            // Kolla om e-post finns
            bool exists = await context.Members.AnyAsync(m => m.Email == member.Email);
            if (exists) throw new Exception("E-postadressen finns redan.");

            context.Members.Add(member);
            await context.SaveChangesAsync();
        }

        // Inloggning
        public async Task<Member?> LoginAsync(string email, string password)
        {
            using var context = _factory.CreateDbContext();
            return await context.Members
                .FirstOrDefaultAsync(m => m.Email == email && m.Password == password);
        }

        // Låna en bok
        public async Task LoanBookAsync(int bookId, int memberId)
        {
            using var context = _factory.CreateDbContext();
            
            var book = await context.Books.FindAsync(bookId);
            if (book != null && book.IsAvailable)
            {
                book.IsAvailable = false;
                context.Loans.Add(new Loan 
                { 
                    BookId = bookId, 
                    MemberId = memberId, 
                    LoanDate = DateTime.Now, 
                    DueDate = DateTime.Now.AddDays(14) 
                });
                await context.SaveChangesAsync();
            }
        }

        // Hämta alla lån för en specifik medlem
        public async Task<List<Loan>> GetLoansForMemberAsync(int memberId)
        {
            using var context = _factory.CreateDbContext();
            return await context.Loans
                .Include(l => l.Book) // Inkludera bokinfo i låneinformationen
                .Where(l => l.MemberId == memberId)
                .ToListAsync();
        }

        // Återlämna en bok
        public async Task ReturnBookAsync(int loanId)
        {
            using var context = _factory.CreateDbContext();
            
            // Hitta lånet och inkludera boken så att vi kan uppdatera dess tillgänglighet
            var loan = await context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan != null && loan.ReturnDate == null)
            {
                // Sätt återlämningsdatum
                loan.ReturnDate = DateTime.Now;

                // Gör boken tillgänglig igen
                if (loan.Book != null)
                {
                    loan.Book.IsAvailable = true;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}