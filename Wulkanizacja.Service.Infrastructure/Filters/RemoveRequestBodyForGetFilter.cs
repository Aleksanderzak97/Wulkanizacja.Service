using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wulkanizacja.Service.Infrastructure.Filters
{
    public class RemoveRequestBodyForGetFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod?.ToUpperInvariant() == "GET")
            {
                operation.RequestBody = null;
            }
        }
    }
}
