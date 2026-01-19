using Elyssa.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EnvironmentEntity = Elyssa.Core.Domain.Entities.Environment;

namespace Elyssa.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; } = null!;
    public virtual DbSet<Country> Countries { get; set; } = null!;
    public virtual DbSet<Property> Properties { get; set; } = null!;
    public virtual DbSet<PropertyType> PropertyTypes { get; set; } = null!;
    public virtual DbSet<Inventory> Inventories { get; set; } = null!;
    public virtual DbSet<EstateAgentInCompany> EstateAgentInCompanies { get; set; } = null!;
    public virtual DbSet<EnvironmentEntity> Environments { get; set; } = null!;
    public virtual DbSet<EnvironmentType> EnvironmentTypes { get; set; } = null!;
    public virtual DbSet<EnvironmentDiagnostic> EnvironmentDiagnostics { get; set; } = null!;
    public virtual DbSet<Item> Items { get; set; } = null!;
    public virtual DbSet<Material> Materials { get; set; } = null!;
    public virtual DbSet<ItemDiagnostic> ItemDiagnostics { get; set; } = null!;
    public virtual DbSet<StakeHolderSignature> StakeHolderSignatures { get; set; } = null!;
    public virtual DbSet<OwnerSignature> OwnerSignatures { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCompany(modelBuilder);
        ConfigureCountry(modelBuilder);
        ConfigureProperty(modelBuilder);
        ConfigurePropertyType(modelBuilder);
        ConfigureInventory(modelBuilder);
        ConfigureEstateAgentInCompany(modelBuilder);
        ConfigureEnvironment(modelBuilder);
        ConfigureEnvironmentType(modelBuilder);
        ConfigureEnvironmentDiagnostic(modelBuilder);
        ConfigureItem(modelBuilder);
        ConfigureMaterial(modelBuilder);
        ConfigureItemDiagnostic(modelBuilder);
        ConfigureSignatures(modelBuilder);
    }

    private void ConfigureCompany(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");
            entity.HasKey(e => e.CompanyId);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId");

            entity.Property(e => e.BusinessName).HasColumnName("BusinessName");
            entity.Property(e => e.Tin).HasColumnName("Tin");
            entity.Property(e => e.Email).HasColumnName("Email");
            entity.Property(e => e.AddressNotification).HasColumnName("AddressNotification");
            entity.Property(e => e.Logo).HasColumnName("Logo");
            entity.Property(e => e.TradeName).HasColumnName("TradeName");
            entity.Property(e => e.CityId).HasColumnName("CityId");
            entity.Property(e => e.Phone).HasColumnName("Phone");
            entity.Property(e => e.PlanType).HasColumnName("PlanType");
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.Coins).HasColumnName("Coins");
            entity.Property(e => e.MaxRentedProperties).HasColumnName("MaxRentedProperties");
            entity.Property(e => e.CreationDate).HasColumnName("CreationDate");
            entity.Property(e => e.UpdateDate).HasColumnName("UpdateDate");
            entity.Property(e => e.CountryId).HasColumnName("CountryId");
            entity.Property(e => e.HasCenterRepair).HasColumnName("HasCenterRepair");

            entity.HasOne(e => e.Country)
                .WithMany(c => c.Companies)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Company_Country");
        });
    }

    private void ConfigureCountry(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");
            entity.HasKey(e => e.CountryId);
            entity.Property(e => e.CountryId).HasColumnName("CountryId");

            entity.Property(e => e.CountryName).HasColumnName("CountryName");
            entity.Property(e => e.Currency).HasColumnName("Currency");
            entity.Property(e => e.CurrencySymbol).HasColumnName("CurrencySymbol");
            entity.Property(e => e.PhoneFormat).HasColumnName("PhoneFormat");
            entity.Property(e => e.PrefixPhone).HasColumnName("PrefixPhone");
            
        });
    }

    private void ConfigureProperty(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Property>(entity =>
        {
            entity.ToTable("Property");
            entity.HasKey(e => e.PropertyId);
            entity.Property(e => e.PropertyId).HasColumnName("PropertyId");

            entity.Property(e => e.Address).HasColumnName("Address").IsRequired();
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId").IsRequired();
            entity.Property(e => e.PropertyTypeId).HasColumnName("PropertyTypeId").IsRequired();
            entity.Property(e => e.BuiltArea).HasColumnName("BuiltArea").HasColumnType("numeric");
            entity.Property(e => e.LotArea).HasColumnName("LotArea").HasColumnType("numeric");
            entity.Property(e => e.Neighborhood).HasColumnName("Neighborhood").IsRequired();
            entity.Property(e => e.Code).HasColumnName("Code");
            entity.Property(e => e.City).HasColumnName("City").HasColumnType("text").IsRequired();
            entity.Property(e => e.IsRented).HasColumnName("IsRented").IsRequired();
            entity.Property(e => e.EstateAgentInCompanyId).HasColumnName("EstateAgentInCompanyId");
            entity.Property(e => e.Latitude).HasColumnName("Latitude").HasColumnType("numeric");
            entity.Property(e => e.Longitude).HasColumnName("Longitude").HasColumnType("numeric");
            entity.Property(e => e.Detail).HasColumnName("Detail").HasColumnType("text");
            entity.Property(e => e.Levels).HasColumnName("Levels").HasDefaultValue(0);
            entity.Property(e => e.Country).HasColumnName("Country").HasColumnType("text");
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.CreationDate).HasColumnName("CreationDate");
            entity.Property(e => e.ModificationDate).HasColumnName("ModificationDate");

            entity.HasOne(e => e.PropertyType)
                .WithMany(pt => pt.Properties)
                .HasForeignKey(e => e.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Property_PropertyType");

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Property_Company");

            entity.HasOne(e => e.EstateAgent)
                .WithMany(ea => ea.Properties)
                .HasForeignKey(e => e.EstateAgentInCompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Property_EstateAgentInCompany");
        });
    }

    private void ConfigurePropertyType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.ToTable("PropertyType");
            entity.HasKey(e => e.PropertyTypeId);
            entity.Property(e => e.PropertyTypeId).HasColumnName("PropertyTypeId");
            entity.Property(e => e.typeName).HasColumnName("typeName").IsRequired();
        });
    }

    private void ConfigureInventory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            entity.HasKey(e => e.InventoryId);
            entity.Property(e => e.InventoryId).HasColumnName("InventoryId");

            entity.Property(e => e.PropertyId).HasColumnName("PropertyId").IsRequired();
            entity.Property(e => e.StakeHolderSignatureId).HasColumnName("StakeHolderSignatureId");
            entity.Property(e => e.RentalPrice).HasColumnName("RentalPrice").HasColumnType("numeric").IsRequired();
            entity.Property(e => e.InventoryType).HasColumnName("InventoryType").IsRequired();
            entity.Property(e => e.EstateAgentInCompanyId).HasColumnName("EstateAgentInCompanyId").IsRequired();
            entity.Property(e => e.IsSigned).HasColumnName("IsSigned").HasDefaultValue(false);
            entity.Property(e => e.PfdUrl).HasColumnName("PfdUrl").HasColumnType("text");
            entity.Property(e => e.AgentSignatureDate).HasColumnName("AgentSignatureDate");
            entity.Property(e => e.IsRemoteSigned).HasColumnName("IsRemoteSigned").HasDefaultValue(false);
            entity.Property(e => e.ApprovalCode).HasColumnName("Approval_Code").HasColumnType("text");
            entity.Property(e => e.SignatureDate).HasColumnName("SignatureDate");
            entity.Property(e => e.PdfUrlRef).HasColumnName("PdfUrlRef").HasColumnType("text");
            entity.Property(e => e.OwnerSignatureDate).HasColumnName("OwnerSignatureDate");
            entity.Property(e => e.OwnerSignatureId).HasColumnName("OwnerSignatureId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.Currency).HasColumnName("Currency").HasColumnType("text");
            entity.Property(e => e.CreationDate).HasColumnName("CreationDate").IsRequired();
            
            entity.HasOne(e => e.Property)
                .WithMany(p => p.Inventories)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Inventory_Property");

            entity.HasOne(e => e.EstateAgent)
                .WithMany(ea => ea.Inventories)
                .HasForeignKey(e => e.EstateAgentInCompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Inventory_EstateAgentInCompany");

            entity.HasOne(e => e.StakeHolderSignature)
                .WithMany(s => s.Inventories)
                .HasForeignKey(e => e.StakeHolderSignatureId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Inventory_StakeHolderSignature");

            entity.HasOne(e => e.OwnerSignature)
                .WithMany(o => o.Inventories)
                .HasForeignKey(e => e.OwnerSignatureId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Inventory_OwnerSignature");
        });
    }

    private void ConfigureEstateAgentInCompany(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstateAgentInCompany>(entity =>
        {
            entity.ToTable("EstateAgentInCompany");
            entity.HasKey(e => e.EstateAgentInCompanyId);
            entity.Property(e => e.EstateAgentInCompanyId).HasColumnName("EstateAgentInCompanyId").ValueGeneratedOnAdd();

            entity.Property(e => e.RegistrationDate).HasColumnName("RegistrationDate").IsRequired();
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();
            entity.Property(e => e.IsAdmin).HasColumnName("IsAdmin").IsRequired();
            entity.Property(e => e.EmployeeName).HasColumnName("EmployeeName").IsRequired();
            entity.Property(e => e.IdentificationNumber).HasColumnName("IdentificationNumber").HasColumnType("text").IsRequired();
            entity.Property(e => e.DocumentType).HasColumnName("DocumentType").IsRequired();
            entity.Property(e => e.Email).HasColumnName("Email").IsRequired();
            entity.Property(e => e.Password).HasColumnName("Password").IsRequired();
            entity.Property(e => e.UrlImage).HasColumnName("UrlImage").HasColumnType("text");
            entity.Property(e => e.UrlSignatureImage).HasColumnName("UrlSignatureImage").HasColumnType("text");
            entity.Property(e => e.TokenRecovery).HasColumnName("TokenRecovery").HasColumnType("text").IsRequired();
            entity.Property(e => e.TokenExpiration).HasColumnName("TokenExpiration");
            entity.Property(e => e.Phone).HasColumnName("Phone");
            entity.Property(e => e.RoleAliasId).HasColumnName("RoleAliasId");
            
            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EstateAgentInCompany_Company");
        });
    }
    private void ConfigureEnvironment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnvironmentEntity>(entity =>
        {
            entity.ToTable("Environment");
            entity.HasKey(e => e.EnvironmentId);
            entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentId");

            entity.Property(e => e.Level).HasColumnName("Level").IsRequired();
            entity.Property(e => e.EnvironmentName).HasColumnName("EnvironmentName").HasColumnType("text").IsRequired();
            entity.Property(e => e.EnvironmentTypeId).HasColumnName("EnvironmentTypeId").IsRequired();
            entity.Property(e => e.PropertyId).HasColumnName("PropertyId").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.Order).HasColumnName("Order").HasDefaultValue(0);
            entity.Property(e => e.NuwweDistributionId).HasColumnName("NuwweDistributionId").HasDefaultValue(0);
            entity.Property(e => e.NuwweInmuebleId).HasColumnName("NuwweInmuebleId").HasDefaultValue(0);
            entity.Property(e => e.NuwweOrden).HasColumnName("NuwweOrden").HasDefaultValue(0);

            entity.HasOne(e => e.EnvironmentType)
                .WithMany(et => et.Environments)
                .HasForeignKey(e => e.EnvironmentTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Environment_EnvironmentType");

            entity.HasOne(e => e.Property)
                .WithMany(p => p.Environments)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Environment_Property");
        });
    }

    private void ConfigureEnvironmentType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnvironmentType>(entity =>
        {
            entity.ToTable("EnvironmentType");
            entity.HasKey(e => e.EnvironmentTypeId);
            entity.Property(e => e.EnvironmentTypeId).HasColumnName("EnvironmentTypeId");
            entity.Property(e => e.EnvironmentTypeName).HasColumnType("text").IsRequired();
            
        });
    }

    private void ConfigureEnvironmentDiagnostic(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnvironmentDiagnostic>(entity =>
        {
            entity.ToTable("EnvironmentDiagnostics");
            entity.HasKey(e => e.EnvinronmentDiagnosticId);
            entity.Property(e => e.EnvinronmentDiagnosticId).HasColumnName("EnvironmentDiagnosticId");

            entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentId").IsRequired();
            entity.Property(e => e.InventoryId).HasColumnName("InventoryId").IsRequired();
            entity.Property(e => e.Observations).HasColumnName("Observations");
            entity.Property(e => e.State).HasColumnName("State");

            entity.HasOne(e => e.Environment)
                .WithMany(env => env.EnvironmentDiagnostics)
                .HasForeignKey(e => e.EnvironmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EnvironmentDiagnostic_Environment");

            entity.HasOne(e => e.Inventory)
                .WithMany(i => i.EnvironmentDiagnostics)
                .HasForeignKey(e => e.InventoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EnvironmentDiagnostic_Inventory");
        });
    }

    private void ConfigureItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.ItemId).HasColumnName("ItemId");
            entity.Property(e => e.ItemName).IsRequired();
            entity.Property(e => e.ImageUrl).HasColumnType("text");
        });
    }

    private void ConfigureMaterial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Material");
            entity.HasKey(e => e.MaterialId);
            entity.Property(e => e.MaterialId).HasColumnName("MaterialId");
            entity.Property(e => e.MaterialName).IsRequired();
            entity.Property(e => e.Description);
        });
    }

    private void ConfigureItemDiagnostic(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemDiagnostic>(entity =>
        {
            entity.ToTable("ItemDiagnostic");
            entity.HasKey(e => e.ItemDiagnosticId);
            entity.Property(e => e.ItemDiagnosticId).HasColumnName("ItemDiagnosticId");

            entity.Property(e => e.MaterialId).HasColumnName("MaterialId");
            entity.Property(e => e.ItemId).HasColumnName("ItemId").IsRequired();
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.Rating).HasColumnName("Rating");
            entity.Property(e => e.EnvironmentDiagnosticId).HasColumnName("EnvironmentDiagnosticId").IsRequired();
            entity.Property(e => e.ItemDiagnosticIdSelfReference).HasColumnName("ItemDiagnosticIdSelfReference");
            entity.Property(e => e.Subject).HasColumnName("Subject").HasColumnType("text");
            entity.Property(e => e.Anottation).HasColumnName("Anottation").HasColumnType("text");
            entity.Property(e => e.IsANew).HasColumnName("IsANew").IsRequired();
            entity.Property(e => e.ToRepair).HasColumnName("ToRepair").IsRequired();
            entity.Property(e => e.Amount).HasColumnName("Amount").HasDefaultValue(0);
            entity.Property(e => e.Order).HasColumnName("Order");
            
            entity.HasOne(e => e.Material)
                .WithMany(m => m.ItemDiagnostics)
                .HasForeignKey(e => e.MaterialId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ItemDiagnostic_Material");

            entity.HasOne(e => e.Item)
                .WithMany(i => i.ItemDiagnostics)
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ItemDiagnostic_Item");

            entity.HasOne(e => e.EnvironmentDiagnostic)
                .WithMany(ed => ed.ItemDiagnostics)
                .HasForeignKey(e => e.EnvironmentDiagnosticId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ItemDiagnostic_EnvironmentDiagnostic");

            entity.HasOne(e => e.ParentItemDiagnostic)
                .WithMany(id => id.ChildItemDiagnostics)
                .HasForeignKey(e => e.ItemDiagnosticIdSelfReference)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ItemDiagnostic_ItemDiagnostic");
        });
    }

    private void ConfigureSignatures(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StakeHolderSignature>(entity =>
        {
            entity.ToTable("StakeHolderSignature");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.SignatureDate);
            
        });

        modelBuilder.Entity<OwnerSignature>(entity =>
        {
            entity.ToTable("OwnerSignature");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.SignatureDate);            
        });
    }
}
