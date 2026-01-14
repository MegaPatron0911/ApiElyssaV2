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
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<EstateAgent> EstateAgents { get; set; }
    public DbSet<EstateAgentInCompany> EstateAgentInCompanies { get; set; }
    public DbSet<PropertyEnvironment> PropertyEnvironments { get; set; }
    public DbSet<EnvironmentDiagnostic> EnvironmentDiagnostics { get; set; }
    public DbSet<ItemDiagnostic> ItemDiagnostics { get; set; }

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

        modelBuilder.Entity<Property>(entity =>
        {
            entity.ToTable("Property");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("PropertyId");
            
            entity.Property(e => e.Code).HasMaxLength(100).HasColumnName("Code");
            entity.Property(e => e.Address).HasMaxLength(500).IsRequired().HasColumnName("Address");
            entity.Property(e => e.City).IsRequired().HasColumnName("City");
            entity.Property(e => e.Country).HasMaxLength(100).HasColumnName("Country");
            entity.Property(e => e.Neighborhood).IsRequired().HasColumnName("Neighborhood");
            entity.Property(e => e.IsRented).HasColumnName("IsRented");
            entity.Property(e => e.BuiltArea).HasColumnType("numeric").HasColumnName("BuiltArea");
            entity.Property(e => e.LotArea).HasColumnType("numeric").HasColumnName("LotArea");
            entity.Property(e => e.Levels).HasColumnName("Levels");
            entity.Property(e => e.Detail).HasColumnName("Detail");
            entity.Property(e => e.Latitude).HasColumnType("numeric(10,7)").HasColumnName("Latitude");
            entity.Property(e => e.Longitude).HasColumnType("numeric(10,7)").HasColumnName("Longitude");
            entity.Property(e => e.PropertyTypeId).HasColumnName("PropertyTypeId");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Property(e => e.UpdatedAt).HasColumnName("ModificationDate");

            entity.HasOne(e => e.PropertyType)
                .WithMany(pt => pt.Properties)
                .HasForeignKey(e => e.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.ToTable("PropertyType");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("PropertyTypeId");
            
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired().HasColumnName("typeName");
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.UpdatedAt);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("InventoryId");
            
            entity.Property(e => e.PropertyId).HasColumnName("PropertyId");
            entity.Property(e => e.InventoryType).HasColumnName("InventoryType");
            entity.Property(e => e.IsSigned).HasColumnName("IsSigned");
            entity.Property(e => e.IsRemoteSigned).HasColumnName("IsRemoteSigned");
            entity.Property(e => e.RentalPrice).HasColumnType("numeric").HasColumnName("RentalPrice");
            entity.Property(e => e.Currency).HasMaxLength(10).HasColumnName("Currency");
            entity.Property(e => e.ApprovalCode).HasColumnName("Approval_Code");
            entity.Property(e => e.AgentSignatureDate).HasColumnName("AgentSignatureDate");
            entity.Property(e => e.OwnerSignatureDate).HasColumnName("OwnerSignatureDate");
            entity.Property(e => e.SignatureDate).HasColumnName("SignatureDate");
            entity.Property(e => e.PdfUrl).HasColumnName("PfdUrl");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Ignore(e => e.UpdatedAt);

            entity.HasOne(e => e.Property)
                .WithMany()
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EstateAgent>(entity =>
        {
            entity.ToTable("EstateAgent");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("EstateAgentId");
            
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired().HasColumnName("FirstName");
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired().HasColumnName("LastName");
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired().HasColumnName("Email");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("Phone");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Ignore(e => e.UpdatedAt);
        });

        modelBuilder.Entity<EstateAgentInCompany>(entity =>
        {
            entity.ToTable("EstateAgentInCompany");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("EstateAgentInCompanyId");
            
            entity.Property(e => e.EstateAgentId).HasColumnName("EstateAgentId");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Ignore(e => e.UpdatedAt);

            entity.HasOne(e => e.EstateAgent)
                .WithMany(ea => ea.CompanyAssociations)
                .HasForeignKey(e => e.EstateAgentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PropertyEnvironment>(entity =>
        {
            entity.ToTable("Environment");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("EnvironmentId");
            
            entity.Property(e => e.PropertyId).HasColumnName("PropertyId");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired().HasColumnName("Name");
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Ignore(e => e.UpdatedAt);

            entity.HasOne(e => e.Property)
                .WithMany()
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EnvironmentDiagnostic>(entity =>
        {
            entity.ToTable("EnvironmentDiagnostics");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("EnvironmentDiagnosticId");
            
            entity.Property(e => e.InventoryId).HasColumnName("InventoryId");
            entity.Property(e => e.Observations).HasColumnName("Observations");
            entity.Property(e => e.State).HasMaxLength(50).HasColumnName("State");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Ignore(e => e.UpdatedAt);
            entity.Ignore(e => e.EnvironmentDiagnosticId);

            entity.HasOne(e => e.Inventory)
                .WithMany()
                .HasForeignKey(e => e.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ItemDiagnostic>(entity =>
        {
            entity.ToTable("ItemDiagnostic");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ItemDiagnosticId");
            
            entity.Property(e => e.EnvironmentDiagnosticId).HasColumnName("EnvironmentDiagnosticId");
            entity.Property(e => e.ItemName).HasMaxLength(200).IsRequired().HasColumnName("ItemName");
            entity.Property(e => e.State).HasMaxLength(50).HasColumnName("State");
            entity.Property(e => e.Quantity).HasColumnName("Quantity");
            entity.Property(e => e.Observations).HasColumnName("Observations");
            entity.Property(e => e.CreatedAt).HasColumnName("CreationDate");
            entity.Ignore(e => e.UpdatedAt);

            entity.HasOne(e => e.EnvironmentDiagnostic)
                .WithMany(ed => ed.ItemDiagnostics)
                .HasForeignKey(e => e.EnvironmentDiagnosticId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
