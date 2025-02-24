using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class TireIdParameterOperationFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Paths.TryGetValue("/tires/{TireId}", out var pathItem))
        {
            foreach (var operation in pathItem.Operations)
            {
                if (operation.Value.Parameters == null)
                {
                    operation.Value.Parameters = new List<OpenApiParameter>();
                }

                operation.Value.Parameters.Add(new OpenApiParameter
                {
                    Name = "TireId",
                    In = ParameterLocation.Path,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string", Format = "uuid" },
                    Description = "Identyfikator opony przekazywany w ścieżce URL (Przykład: a3858536-1687-4452-9e77-7a00599f5cb3 << Guid)"
                });
            }
        }
    }
}
