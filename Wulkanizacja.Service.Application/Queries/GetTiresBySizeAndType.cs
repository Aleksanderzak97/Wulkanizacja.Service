using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Application.Queries
{
    public class GetTiresBySizeAndType : IQuery<IEnumerable<TireDto>>
    {
        [FromQuery(Name = "Size")]
        public string Size { get; set; }
        [FromQuery(Name = "TireType")]
        public TireType TireType { get; set; }
    }
}
