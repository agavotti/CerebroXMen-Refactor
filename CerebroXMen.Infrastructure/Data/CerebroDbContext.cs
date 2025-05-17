using CerebroXMen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CerebroXMen.Infrastructure.Data;

public class CerebroDbContext(DbContextOptions<CerebroDbContext> options) : DbContext(options)
{
    public DbSet<DnaSequence> DnaSequences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DnaSequence>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sequence)
                  .HasColumnType("text[]")
                  .IsRequired(); 
            entity.Property(e => e.IsMutant).IsRequired();
            entity.Property(e => e.FechaAlta).IsRequired();

        });
    }
}