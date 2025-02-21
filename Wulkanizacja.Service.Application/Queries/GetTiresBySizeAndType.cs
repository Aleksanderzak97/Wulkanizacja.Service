using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Application.Queries
{
    public class GetTiresBySizeAndType : IQuery<IEnumerable<TireDto>>
    {
        [FromHeader(Name = "Size")]
        public string Size { get; set; }
        [FromHeader(Name = "TireType")]
        public TireType TireType { get; set; }
    }
}
