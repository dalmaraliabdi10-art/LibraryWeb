using Microsoft.EntityFrameworkCore;
using LibraryWeb.Models;

namespace LibraryWeb.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        // DbSet för varje modell
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Loan> Loans { get; set; }

        // Seeda datan
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeda Böcker
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Sagan om ringen", Author = "Tolkien", ISBN = "123", PublishedYear = 1954 },
                new Book { Id = 2, Title = "Harry Potter", Author = "Rowling", ISBN = "456", PublishedYear = 1997 }
            );

            // Seeda Admin och Medlem
            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, Name = "Admin", Email = "admin@admin.se", Password = "admin", IsAdmin = true },
                // Seeda en vanlig medlem
                new Member { Id = 2, Name = "qwe", Email = "qwe@mail.se", Password = "123", IsAdmin = false }
            );
        }
    }
}