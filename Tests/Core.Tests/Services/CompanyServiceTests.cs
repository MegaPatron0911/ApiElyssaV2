using AutoMapper;
using Elyssa.Core.Common;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.Core.Mappings;
using Elyssa.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Core.Tests.Services;

public class CompanyServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Company>> _mockRepository;
    private readonly IMapper _mapper;
    private readonly CompanyService _sut; // System Under Test

    public CompanyServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<IRepository<Company>>();
        
        // Configurar AutoMapper con el perfil real
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CompanyMappingProfile>();
        });
        _mapper = config.CreateMapper();
        
        _mockUnitOfWork.Setup(x => x.Companies).Returns(_mockRepository.Object);
        
        _sut = new CompanyService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyExists_ReturnsSuccess()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = new Company
        {
            Id = companyId,
            Name = "Test Company",
            Email = "test@company.com",
            Phone = "1234567890",
            Description = "Test Description",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository
            .Setup(x => x.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        // Act
        var result = await _sut.GetByIdAsync(companyId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(companyId);
        result.Value.Name.Should().Be("Test Company");
        result.Value.Email.Should().Be("test@company.com");
    }

    [Fact]
    public async Task GetByIdAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        
        _mockRepository
            .Setup(x => x.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act
        var result = await _sut.GetByIdAsync(companyId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        result.Error.Code.Should().Be("Company.NotFound");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCompanies()
    {
        // Arrange
        var companies = new List<Company>
        {
            new() { Id = Guid.NewGuid(), Name = "Company 1", Email = "c1@test.com", IsActive = true },
            new() { Id = Guid.NewGuid(), Name = "Company 2", Email = "c2@test.com", IsActive = true }
        };

        _mockRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(companies);

        // Act
        var result = await _sut.GetAllAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var dto = new CompanyDto
        {
            Name = "New Company",
            Email = "new@company.com",
            Phone = "1234567890",
            Description = "New Description",
            IsActive = true
        };

        _mockRepository
            .Setup(x => x.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company c, CancellationToken ct) => c);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.CreateAsync(dto, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("New Company");
        result.Value.Email.Should().Be("new@company.com");
        
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("AB")] // Menos de 3 caracteres
    public async Task CreateAsync_WithInvalidName_ReturnsFailure(string invalidName)
    {
        // Arrange
        var dto = new CompanyDto
        {
            Name = invalidName,
            Email = "test@company.com",
            IsActive = true
        };

        // Act
        var result = await _sut.CreateAsync(dto, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be("Company.NameTooShort");
        
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenCompanyExists_ReturnsSuccess()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var existingCompany = new Company
        {
            Id = companyId,
            Name = "Old Name",
            Email = "old@email.com",
            IsActive = true
        };

        var dto = new CompanyDto
        {
            Name = "Updated Name",
            Email = "new@email.com",
            Phone = "9876543210",
            Description = "Updated Description",
            IsActive = false
        };

        _mockRepository
            .Setup(x => x.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.UpdateAsync(companyId, dto, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var dto = new CompanyDto { Name = "Test", Email = "test@email.com" };

        _mockRepository
            .Setup(x => x.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // Act
        var result = await _sut.UpdateAsync(companyId, dto, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenCompanyExists_ReturnsSuccess()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        _mockRepository
            .Setup(x => x.ExistsAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.DeleteAsync(companyId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        _mockRepository.Verify(x => x.DeleteAsync(companyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        _mockRepository
            .Setup(x => x.ExistsAsync(companyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.DeleteAsync(companyId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Type.Should().Be(ErrorType.NotFound);
        
        _mockRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
