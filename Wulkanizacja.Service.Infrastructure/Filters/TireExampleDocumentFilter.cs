using Convey.WebApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Infrastructure.Filters
{
    public class TireExampleDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc.Paths.TryGetValue("/tires", out var pathItem))
            {
                if (pathItem.Operations.TryGetValue(OperationType.Post, out var operation))
                {
                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(TireDto), context.SchemaRepository),
                                Examples = new Dictionary<string, OpenApiExample>
                                {
                                    ["TireExample"] = new OpenApiExample
                                    {
                                        Value = new OpenApiObject
                                        {
                                            ["Tire"] = new OpenApiObject
                                            {
                                                ["Brand"] = new OpenApiString("Michelin"),
                                                ["Model"] = new OpenApiString("Pilot Sport 4"),
                                                ["Size"] = new OpenApiString("205/55 R16"),
                                                ["SpeedIndex"] = new OpenApiString("Y"),
                                                ["LoadIndex"] = new OpenApiString("91"),
                                                ["TireType"] = new OpenApiInteger((int)TireType.Summer),
                                                ["ManufactureDate"] = new OpenApiString("2025-01-01T00:00:00Z"),
                                                ["ManufactureWeekYear"] = new OpenApiString("5224"),
                                                ["Comments"] = new OpenApiString("Przykładowe komentarze"),
                                                ["QuantityInStock"] = new OpenApiInteger((int)10)
                                            }
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
