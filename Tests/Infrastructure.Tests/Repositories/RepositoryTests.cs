using Elyssa.Core.Domain.Entities;
using Elyssa.Infrastructure.Data;
using Elyssa.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class RepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Repository<Company> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new Repository<Company>(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Test Company",
            Email = "test@company.com",
            IsActive = true
        };

        // Act
        var result = await _repository.AddAsync(company, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(company.Id);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        var savedCompany = await _context.Companies.FindAsync(company.Id);
        savedCompany.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ReturnsEntity()
    {
        // Arrange
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Test Company",
            Email = "test@company.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(company.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(company.Id);
        result.Name.Should().Be("Test Company");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ReturnsNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Arrange
        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), Name = "Company 1", Email = "c1@test.com", IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Company 2", Email = "c2@test.com", IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Company 3", Email = "c3@test.com", IsActive = false, CreatedAt = DateTime.UtcNow }
        };

        await _context.Companies.AddRangeAsync(companies);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(c => c.Name == "Company 1");
        result.Should().Contain(c => c.Name == "Company 2");
        result.Should().Contain(c => c.Name == "Company 3");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Email = "original@email.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        // Act
        company.Name = "Updated Name";
        company.Email = "updated@email.com";
        await _repository.UpdateAsync(company, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var updatedCompany = await _context.Companies.FindAsync(company.Id);
        updatedCompany.Should().NotBeNull();
        updatedCompany!.Name.Should().Be("Updated Name");
        updatedCompany.Email.Should().Be("updated@email.com");
        updatedCompany.UpdatedAt.Should().NotBeNull();
        updatedCompany.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            Email = "delete@email.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(company.Id, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var deletedCompany = await _context.Companies.FindAsync(company.Id);
        deletedCompany.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_WhenExists_ReturnsTrue()
    {
        // Arrange
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Exists",
            Email = "exists@email.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(company.Id, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenNotExists_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.ExistsAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
