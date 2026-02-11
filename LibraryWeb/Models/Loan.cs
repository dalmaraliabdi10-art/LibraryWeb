using System;

namespace LibraryWeb.Models
{
    // Låneinformation
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
        public int MemberId { get; set; }
        public virtual Member? Member { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Kontrollera om lånet är försenat
        public bool IsOverdue()
        {
            if (ReturnDate.HasValue) return false;
            return DateTime.Now > DueDate;
        }
    }
}