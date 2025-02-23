using System.Net;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Convey.Docs.Swagger;
using Convey.Logging;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using Wulkanizacja.Service.Application;
using Wulkanizacja.Service.Application.Commands;
using Wulkanizacja.Service.Application.Converters;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Application.Queries;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Infrastructure;
using Wulkanizacja.Service.Infrastructure.Filters;
using Wulkanizacja.Service.Infrastructure.Postgres.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<PostgresOptions>(builder.Configuration.GetSection("postgres"));
builder.Services.AddSingleton<WeekYearToDateConverter>();
builder.Services.AddControllers();

// Rejestracja usług
builder.Services
    .AddConvey()
    .AddWebApi()
    .AddApplication()
    .AddMessaging()
    .AddPostgres()
    .AddSwagger()
    .Build();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Wulkanizacja Service API",
        Description = "API for Wulkanizacja Service"
    });
    c.DocumentFilter<TireExampleDocumentFilter>();
    c.DocumentFilter<UpdateTireExampleDocumentFilter>();
    c.DocumentFilter<TireSizeTypeParameterDocumentFilter>();
    c.DocumentFilter<TireIdPathParameterDocumentFilter>();
    c.DocumentFilter<DeleteTirePathParameterDocumentFilter>();
    c.ExampleFilters();
    c.OperationFilter<SwaggerHeaderFilter>();


});

builder.Services.AddSwaggerExamplesFromAssemblyOf<TireDtoExamples>();


var app = builder.Build();

// Middleware i konfiguracja aplikacji

app.UseSwaggerDocs();
app.UseApplication();
app.UseDispatcherEndpoints(endpoints => endpoints
    .Post<PostTire>("tires",
        endpoint: endpoint => endpoint
        .WithDescription("Dodaje nową oponę"),

        beforeDispatch: (cmd, httpContext) =>
        {
            return Task.CompletedTask;
        },
        afterDispatch: async (_, httpContext) =>
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Created;
        })

    .Get("tires/size/{Size}/TireType/{TireType}",
        context: async httpContext =>
        {
            var encodedSize = httpContext.Request.RouteValues["Size"]?.ToString();

            string? tireTypeString = httpContext.Request.RouteValues["TireType"]?.ToString();

            var size = Uri.UnescapeDataString(encodedSize ?? "");

            if (string.IsNullOrEmpty(size))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Brak wymaganego parametru `Size`." });
                return;
            }

            if (string.IsNullOrEmpty(tireTypeString) || !Enum.TryParse<TireType>(tireTypeString, out var tireType))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Niepoprawna wartość `TireType`." });
                return;
            }

            var query = new GetTiresBySizeAndType
            {
                Size = size,
                TireType = tireType
            };

            var dispatcher = httpContext.RequestServices.GetRequiredService<IQueryDispatcher>();
            var result = await dispatcher.QueryAsync(query);

            await httpContext.Response.WriteAsJsonAsync(result);
        },
        endpoint: endpoint => endpoint
            .WithDescription("Pobiera opony na podstawie rozmiaru i typu")
            .WithSummary("Pobiera listę opon o podanym rozmiarze i typie")
    )

    .Get("tires/{TireId}",
        context: async httpContext =>
        {
            var tireIdString = httpContext.Request.RouteValues["TireId"]?.ToString();
            if (string.IsNullOrEmpty(tireIdString) || !Guid.TryParse(tireIdString, out var tireId))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Niepoprawny identyfikator opony." });
                return;
            }

            var query = new GetTireById { TireId = tireId };

            var dispatcher = httpContext.RequestServices.GetRequiredService<IQueryDispatcher>();
            var result = await dispatcher.QueryAsync(query);

            if (result == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Opona nie została znaleziona." });
                return;
            }

            await httpContext.Response.WriteAsJsonAsync(result);
        },
        endpoint: endpoint => endpoint
            .WithDescription("Pobiera oponę na podstawie TireId")
            .WithSummary("Pobiera dane konkretnej opony")
 
    )

    .Put("tires/updateTire/{TireId}",
        context: async httpContext =>
        {
            var tireIdString = httpContext.Request.RouteValues["TireId"]?.ToString();
            if (string.IsNullOrEmpty(tireIdString) || !Guid.TryParse(tireIdString, out var tireId))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Niepoprawny identyfikator opony." });
                return;
            }

            var updateTireDto = await httpContext.Request.ReadFromJsonAsync<PutTire>();
            if (updateTireDto == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Brak danych do aktualizacji." });
                return;
            }

            updateTireDto.SetTireId(tireId);

            var dispatcher = httpContext.RequestServices.GetRequiredService<ICommandDispatcher>();
            await dispatcher.SendAsync(updateTireDto);

            httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
        },
        endpoint: endpoint => endpoint.WithDescription("Aktualizuje oponę na podstawie TireId"))


    .Delete("tires/{TireId}/removeTire",
        context: async httpContext =>
        {
            var tireIdString = httpContext.Request.RouteValues["TireId"]?.ToString();
            if (string.IsNullOrEmpty(tireIdString) || !Guid.TryParse(tireIdString, out var tireId))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Niepoprawny identyfikator opony." });
                return;
            }

            var command = new DeleteTire(tireId);
            var dispatcher = httpContext.RequestServices.GetRequiredService<ICommandDispatcher>();

            await dispatcher.SendAsync(command);
            httpContext.Response.StatusCode = StatusCodes.Status202Accepted;
        },
        endpoint: endpoint => endpoint.WithDescription("Usuwa oponę na podstawie TireId"))
);

app.Run();
