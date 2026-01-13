using Asp.Versioning;
using Elyssa.Core.Interfaces;
using Elyssa.Core.Services;
using Elyssa.Core.Validators;
using Elyssa.Infrastructure.Data;
using Elyssa.Infrastructure.Repositories;
using Elyssa.PublicApi.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/elyssa-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Iniciando Elyssa API...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"),
            new QueryStringApiVersionReader("api-version")
        );
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddControllers();
    
    builder.Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters();
    
    builder.Services.AddValidatorsFromAssemblyContaining<CompanyDtoValidator>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title = "Elyssa API",
            Version = "v2",
            Description = "API BackOffice Elyssa",
            Contact = new()
            {
                Name = "Equipo Elyssa",
                Email = "oswaldo@elyssa.app"
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    builder.Services.AddProblemDetails();
    builder.Services.AddMemoryCache();
    builder.Services.AddAutoMapper(
        typeof(Elyssa.Core.Mappings.CompanyMappingProfile),
        typeof(Elyssa.Core.Mappings.PropertyMappingProfile));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("deployDatabase"),
            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<ICompanyService, CompanyService>();
    builder.Services.AddScoped<IPropertyService, PropertyService>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandlingMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elyssa API v1");
            c.RoutePrefix = "swagger";
            c.DocumentTitle = "Elyssa API - Documentación";
        });
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Elyssa API iniciada exitosamente");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
