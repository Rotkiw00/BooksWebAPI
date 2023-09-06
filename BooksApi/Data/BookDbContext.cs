using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .Property(e => e.Genre)
                .HasConversion<string>();
        }

        public DbSet<Book> Books { get; set; }
    }
}
