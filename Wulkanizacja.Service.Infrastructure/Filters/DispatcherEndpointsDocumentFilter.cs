using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Wulkanizacja.Service.Application.Commands;

namespace Wulkanizacja.Service.Infrastructure.Filters
{
    public class DispatcherEndpointsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Dodaj ręcznie endpointy zarejestrowane przez UseDispatcherEndpoints
            swaggerDoc.Paths.Add("/tires", new OpenApiPathItem
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Post] = new OpenApiOperation
                    {
                        Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Tires" } },
                        Summary = "Dodaje nową oponę",
                        OperationId = "PostTire",
                        RequestBody = new OpenApiRequestBody
                        {
                            Content = new Dictionary<string, OpenApiMediaType>
                            {
                                ["application/json"] = new OpenApiMediaType
                                {
                                    Schema = context.SchemaGenerator.GenerateSchema(typeof(PostTire), context.SchemaRepository)
                                }
                            }
                        },
                        Responses = new OpenApiResponses
                        {
                            ["201"] = new OpenApiResponse { Description = "Created" }
                        }
                    }
                }
            });
        }
    }
}
