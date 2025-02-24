using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Filters
{
    public class SizeAndTireTypeParameterDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc.Paths.TryGetValue("/tires/size/{Size}/TireType/{TireType}", out var pathItem))
            {
                foreach (var operation in pathItem.Operations)
                {
                    if (operation.Value.Parameters == null)
                    {
                        operation.Value.Parameters = new List<OpenApiParameter>();
                    }

                    operation.Value.Parameters.Add(new OpenApiParameter
                    {
                        Name = "Size",
                        In = ParameterLocation.Path,
                        Required = true,
                        Schema = new OpenApiSchema { Type = "string"},
                        Description = "Rozmiar opony przekazywany w ścieżce URL (Przykład: 205/55 R16)"
                    });

                    operation.Value.Parameters.Add(new OpenApiParameter
                    {
                        Name = "TireType",
                        In = ParameterLocation.Path,
                        Required = true,
                        Schema = new OpenApiSchema { Type = "string" },
                        Description = "Typ opony przekazywany w ścieżce URL (Przykład: 1)"
                    });
                }
            }
        }
    }
}
