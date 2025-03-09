using Swashbuckle.AspNetCore.Filters;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Infrastructure.Filters
{
    public class TireDtoExamples : IExamplesProvider<TireDto>
    {
        public TireDto GetExamples()
        {
            return new TireDto
            {
                Id = Guid.NewGuid(),
                Brand = "Michelin",
                Model = "Pilot Sport 4",
                Size = "205/55 R16",
                SpeedIndex = "Y",
                LoadIndex = "91",
                TireType = TireType.Summer,
                ManufactureDate = "5224",
                CreateDate = DateTimeOffset.Parse("2025-01-01T00:00:00Z"),
                EditDate = DateTimeOffset.Parse("2025-01-01T00:00:00Z"),
                Comments = "Przykładowe komentarze",
                QuantityInStock = 10
            };
        }
    }
}
