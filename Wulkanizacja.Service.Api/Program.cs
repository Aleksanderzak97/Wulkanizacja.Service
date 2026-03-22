using System.Globalization;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Wulkanizacja.Service.Api.Exceptions;
using Wulkanizacja.Service.Application;
using Wulkanizacja.Service.Application.CQRS.Commands;
using Wulkanizacja.Service.Application.CQRS.Queries;
using Wulkanizacja.Service.Application.Commands;
using Wulkanizacja.Service.Application.Queries;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Infrastructure;
using Wulkanizacja.Service.Infrastructure.Exceptions;
using Wulkanizacja.Service.Infrastructure.Postgres.Services;


var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Brak konfiguracji Jwt:Key.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddControllers();

// Rejestracja usług
builder.Services
    .AddApplication()
    .AddMessaging()
    .AddPostgres(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Wulkanizacja Service API",
        Description = "API for Wulkanizacja Service"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Wprowadź token Bearer. Przykład: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var systemLanguage = CultureInfo.InstalledUICulture.Name;
var culture = new CultureInfo(systemLanguage);
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
    await migrationService.EnsureMigrationsAppliedAsync();
}

// Middleware i konfiguracja aplikacji
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("tires", async (PostTire command, ICommandDispatcher dispatcher, HttpContext httpContext) =>
    {
        if (command.Tire == null || command.Tire.Validate())
        {
            throw new EmptyPostDataException("Brak danych do utworzenia opony.");
        }

        await dispatcher.SendAsync(command, httpContext.RequestAborted);
        return Results.Created("/tires", new { message = "Opona została utworzona." });
    })
    .WithDescription("Dodaje nową oponę")
    .RequireAuthorization();

app.MapGet("tires/size/{Size}/TireType/{TireType}", async (string size, string tireType, IQueryDispatcher dispatcher, HttpContext httpContext) =>
    {
        var decodedSize = Uri.UnescapeDataString(size ?? string.Empty);
        if (string.IsNullOrWhiteSpace(decodedSize))
        {
            throw new EmptyTireSizeException("Brak wymaganego parametru 'Size'.");
        }

        if (!Enum.TryParse<TireType>(tireType, true, out var parsedType) || !Enum.IsDefined(typeof(TireType), parsedType))
        {
            throw new InvalidTireTypeException($"Niepoprawna wartość `TireType`: {tireType}");
        }

        var query = new GetTiresBySizeAndType
        {
            Size = decodedSize,
            TireType = parsedType
        };

        var result = await dispatcher.QueryAsync(query, httpContext.RequestAborted);
        return Results.Ok(result);
    })
    .WithDescription("Pobiera opony na podstawie rozmiaru i typu")
    .WithSummary("Pobiera listę opon o podanym rozmiarze i typie")
    .RequireAuthorization();

app.MapGet("tires/{TireId}", async (string tireId, IQueryDispatcher dispatcher, HttpContext httpContext) =>
    {
        if (!Guid.TryParse(tireId, out var parsedTireId))
        {
            throw new BadIdentifierException("Niepoprawny identyfikator opony.");
        }

        var query = new GetTireById { TireId = parsedTireId };
        var result = await dispatcher.QueryAsync(query, httpContext.RequestAborted);

        return Results.Ok(result);
    })
    .WithDescription("Pobiera oponę na podstawie TireId")
    .WithSummary("Pobiera dane konkretnej opony")
    .RequireAuthorization();

app.MapPut("tires/updateTire/{TireId}", async (string tireId, PutTire updateTireDto, ICommandDispatcher dispatcher, HttpContext httpContext) =>
    {
        if (!Guid.TryParse(tireId, out var parsedTireId))
        {
            throw new BadIdentifierException("Niepoprawny identyfikator opony.");
        }

        if (updateTireDto == null || updateTireDto.IsEmpty())
        {
            throw new EmptyUpdateDataException("Brak danych do aktualizacji.");
        }

        updateTireDto.SetTireId(parsedTireId);
        await dispatcher.SendAsync(updateTireDto, httpContext.RequestAborted);
        return Results.NoContent();
    })
    .WithDescription("Aktualizuje oponę na podstawie TireId")
    .RequireAuthorization();

app.MapDelete("tires/{TireId}/removeTire", async (string tireId, ICommandDispatcher dispatcher, HttpContext httpContext) =>
    {
        if (!Guid.TryParse(tireId, out var parsedTireId))
        {
            throw new BadIdentifierException("Niepoprawny identyfikator opony.");
        }

        var command = new DeleteTire(parsedTireId);
        await dispatcher.SendAsync(command, httpContext.RequestAborted);
        return Results.Accepted();
    })
    .WithDescription("Usuwa oponę na podstawie TireId")
    .RequireAuthorization();

app.Run();
