using Microsoft.EntityFrameworkCore;

namespace MyProject.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TitleTag> TitlesTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./data/books.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Renombrar tabla TitleTag a TitlesTags
            modelBuilder.Entity<TitleTag>().ToTable("TitlesTags");

            // Ordenar columnas de Title: TitleId, AuthorId, TitleName
            modelBuilder.Entity<Title>()
                .Property(t => t.TitleId)
                .HasColumnOrder(0);

            modelBuilder.Entity<Title>()
                .Property(t => t.AuthorId)
                .HasColumnOrder(1);

            modelBuilder.Entity<Title>()
                .Property(t => t.TitleName)
                .HasColumnOrder(2);

            // Todas las propiedades requeridas (NOT NULL)
            modelBuilder.Entity<Author>()
                .Property(a => a.AuthorName)
                .IsRequired();

            modelBuilder.Entity<Title>()
                .Property(t => t.TitleName)
                .IsRequired();

            modelBuilder.Entity<Tag>()
                .Property(t => t.TagName)
                .IsRequired();

            modelBuilder.Entity<TitleTag>()
                .Property(tt => tt.TitleId)
                .IsRequired();

            modelBuilder.Entity<TitleTag>()
                .Property(tt => tt.TagId)
                .IsRequired();
        }
    }
}