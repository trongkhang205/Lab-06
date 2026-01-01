using System.Data.Entity;
using Lab_06.DTO;

namespace Lab_06.DAL
{
    public class BookstoreContext : DbContext
    {
        // Constructor reads the connection string from DatabaseAccess which manages the text file
        public BookstoreContext() : base(DatabaseAccess.ConnectionString)
        {
            // Initializer to avoid creating DB if exists, or simple null
            Database.SetInitializer<BookstoreContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanDetail> LoanDetails { get; set; }
    }
}
