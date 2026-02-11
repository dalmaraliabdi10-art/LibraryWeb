using Xunit;
using LibraryWeb.Models;
using LibraryWeb.Services;
using LibraryWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LibraryWeb.Tests
{
    public class LibraryTests
    {
        // Testa att Book-klassen sätter egenskaper korrekt

        [Fact] 
        public void Book_Should_SetPropertiesCorrectly()
        {
            var book = new Book { Title = "Testbok", Author = "Testare", PublishedYear = 2024 };
            Assert.Equal("Testbok", book.Title);
            Assert.True(book.IsAvailable); 
        }

        [Theory]
        [InlineData("Harry", true)]
        [InlineData("rowling", true)]
        [InlineData("978", true)]
        [InlineData("Zlatan", false)]
        public void Book_Matches_Should_ReturnCorrectResult(string term, bool expected)
        {
            var book = new Book { Title = "Harry Potter", Author = "JK Rowling", ISBN = "978-123" };
            Assert.Equal(expected, book.Matches(term));
        }

        [Fact]
        public void Book_Matches_Should_ReturnFalse_ForEmptyString()
        {
            var book = new Book { Title = "A" };
            Assert.False(book.Matches(""));
            // Också testa null eller whitespace
            Assert.False(book.Matches(string.Empty)); 
        }

        // Testa att Member-klassen sätter egenskaper korrekt

        [Fact]
        public void Loan_IsOverdue_Should_ReturnTrue_IfDueDatePassed()
        {
            var loan = new Loan 
            { 
                LoanDate = DateTime.Now.AddDays(-20), 
                DueDate = DateTime.Now.AddDays(-5) 
            };
            Assert.True(loan.IsOverdue());
        }

        [Fact]
        public void Loan_IsOverdue_Should_ReturnFalse_IfFuture()
        {
            var loan = new Loan { DueDate = DateTime.Now.AddDays(5) };
            Assert.False(loan.IsOverdue());
        }

        // För att testa LibraryService behöver vi en riktig databas, så vi använder en InMemory-databas som är unik för varje test.

        // För att testa LibraryService behöver vi en riktig databas
        private DbContextOptions<LibraryContext> GetOptions()
        {
            return new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unik DB för varje test
                .Options;
        }

        [Fact] // Testa att lägga till en bok och att den finns i databasen
        public async Task AddBookAsync_Should_AddBookToDatabase()
        {
            // Arrange
            var options = GetOptions();
            var factory = new TestDbContextFactory(options); // Vi använder vår TestDbContextFactory för att skapa nya DbContext-instans varje gång
            var service = new LibraryService(factory);
            var book = new Book { Title = "Ny Bok" };

            // Act
            await service.AddBookAsync(book);

            // Assert
            using var context = new LibraryContext(options);
            Assert.Equal(1, await context.Books.CountAsync());
        }

        [Fact] // Testa att låna en bok och att den blir otillgänglig
        public async Task LoanBookAsync_Should_SetBookToUnavailable()
        {
            // Arrange
            var options = GetOptions();
            var factory = new TestDbContextFactory(options);
            
            // Seeda en bok som är tillgänglig
            using (var seedContext = new LibraryContext(options))
            {
                seedContext.Books.Add(new Book { Id = 1, Title = "Bok", IsAvailable = true });
                await seedContext.SaveChangesAsync();
            }

            var service = new LibraryService(factory);

            // Act
            await service.LoanBookAsync(1, 100);

            // Assert
            using var verifyContext = new LibraryContext(options);
            var dbBook = await verifyContext.Books.FindAsync(1);
            Assert.False(dbBook!.IsAvailable);
            Assert.Single(verifyContext.Loans);
        }

        [Fact]
        public async Task RegisterMember_Should_Throw_If_Email_Exists()
        {
            // Arrange
            var options = GetOptions();
            var factory = new TestDbContextFactory(options);

            // Seeda en medlem
            using (var seedContext = new LibraryContext(options))
            {
                seedContext.Members.Add(new Member { Name="A", Email="test@test.se", Password="123" });
                await seedContext.SaveChangesAsync();
            }

            var service = new LibraryService(factory);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => 
            {
                await service.RegisterMemberAsync(new Member { Name="B", Email="test@test.se", Password="456" });
            });
        }
    }

    // En enkel "låtsas-fabrik" som implementerar IDbContextFactory för att skapa nya DbContext-instans varje gång.
    // Detta är nödvändigt för att våra tester ska fungera korrekt, eftersom varje test behöver en fräsch databasinstans.
    public class TestDbContextFactory : IDbContextFactory<LibraryContext>
    {
        private readonly DbContextOptions<LibraryContext> _options;

        public TestDbContextFactory(DbContextOptions<LibraryContext> options)
        {
            _options = options;
        }

        public LibraryContext CreateDbContext()
        {
            return new LibraryContext(_options);
        }
    }
}