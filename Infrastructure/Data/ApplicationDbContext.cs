using Elyssa.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elyssa.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("CompanyId");
            
            entity.Property(e => e.Name).HasMaxLength(200).HasColumnName("TradeName");
            entity.Property(e => e.BusinessName).HasMaxLength(200).HasColumnName("BusinessName");
            entity.Property(e => e.Nit).HasMaxLength(50).HasColumnName("Tin");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("Email");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("Phone");
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.PlanType).HasColumnName("PlanType");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Property(e => e.UpdatedAt).HasColumnName("UpdateDate");
            
            entity.Ignore(e => e.IsActive);
            entity.Ignore(e => e.Description);
        });
    }
}
