using Convey.WebApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Wulkanizacja.Service.Application.Commands;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Infrastructure.Filters
{
    public class UpdateTireExampleDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc.Paths.TryGetValue("/tires/updateTire/{TireId}", out var pathItem))
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
                if (pathItem.Operations.TryGetValue(OperationType.Put, out var putOperation))
                {
                    putOperation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(PutTire), context.SchemaRepository),
                                Examples = new Dictionary<string, OpenApiExample>
                                {
                                    ["TireUpdateExample"] = new OpenApiExample
                                    {
                                        Value = new OpenApiObject
                                        {
                                            ["Brand"] = new OpenApiString("Michelin"),
                                            ["Model"] = new OpenApiString("Pilot Sport 4"),
                                            ["Size"] = new OpenApiString("205/55 R16"),
                                            ["SpeedIndex"] = new OpenApiString("Y"),
                                            ["LoadIndex"] = new OpenApiString("91"),
                                            ["TireType"] = new OpenApiInteger((int)TireType.Summer),
                                            ["ManufactureWeekYear"] = new OpenApiString("5224"),
                                            ["Comments"] = new OpenApiString("Przykładowe komentarze"),
                                            ["QuantityInStock"] = new OpenApiInteger(10)
                                        }
                                    }
                                }
                            }
                        }
                    };
                }
            }
        }
    }
}
