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
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Application.Queries;
using Wulkanizacja.Service.Core.Enums;
using Wulkanizacja.Service.Infrastructure;
using Wulkanizacja.Service.Infrastructure.Filters;
using Wulkanizacja.Service.Infrastructure.Postgres.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<PostgresOptions>(builder.Configuration.GetSection("postgres"));



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
    c.OperationFilter<RemoveRequestBodyForGetFilter>();

    c.DocumentFilter<TireDtoExamplesDocumentFilter>();
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

    .Get<GetTiresBySizeAndType, IEnumerable<TireDto>>("tires/search",
        endpoint: endpoint => endpoint.WithDescription("Pobiera opony na podstawie rozmiaru i typu"),
        beforeDispatch: (cmd, httpContext) =>
        {
            cmd.Size = httpContext.Request.Headers["Size"].ToString();
            cmd.TireType = Enum.Parse<TireType>(httpContext.Request.Headers["TireType"].ToString());
            return Task.CompletedTask;
        })

//.Get<GetTire, TireDto>("tires/{id}",
//    endpoint: endpoint => endpoint.WithDescription("Pobiera oponę na podstawie ID"))

//.Get<GetAllTires, IEnumerable<TireDto>>("tires",
//    endpoint: endpoint => endpoint.WithDescription("Pobiera wszystkie opony"))

//.Put<UpdateTire>("tires/update",
//    endpoint: endpoint => endpoint.WithDescription("Aktualizuje oponę"))

//.Delete<DeleteTire>("tires/{id}/delete",
//    endpoint: endpoint => endpoint.WithDescription("Usuwa oponę o podanym ID"))
);

app.Run();
