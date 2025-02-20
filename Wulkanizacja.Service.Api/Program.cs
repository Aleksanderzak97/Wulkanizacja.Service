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
using Microsoft.Extensions.DependencyInjection;
using Wulkanizacja.Service.Application;
using Wulkanizacja.Service.Application.Commands;
using Wulkanizacja.Service.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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



var app = builder.Build();
//app.MapControllers();


// Middleware i konfiguracja aplikacji

app.UseSwaggerDocs();
app.UseApplication();
app.UsePublicContracts<ContractAttribute>();
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
