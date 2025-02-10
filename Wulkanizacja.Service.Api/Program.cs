using Convey;
using Convey.Logging;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja Convey
builder.Services
    .AddConvey()
    .AddWebApi()
    .Build();

var app = builder.Build();

// Middleware
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
