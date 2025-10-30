using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data;

public class URLShortenerContext : DbContext
{
    public URLShortenerContext(DbContextOptions<URLShortenerContext> options) : base(options) { }

    public DbSet<URLMap> URLMaps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<URLMap>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Url).IsRequired().HasMaxLength(2048);
                entity.Property(u => u.ShortCode).IsRequired().HasMaxLength(10);
                entity.HasIndex(u => u.ShortCode).IsUnique();
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("datetime('now')");
                entity.Property(u => u.UpdatedAt).HasDefaultValueSql("datetime('now')");
                entity.Property(u => u.AccessCount).HasDefaultValue(0);
            });

        base.OnModelCreating(modelBuilder);
    }
}
