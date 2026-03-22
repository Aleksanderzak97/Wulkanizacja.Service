using Wulkanizacja.Service.Application.CQRS.Queries;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Application.Queries
{
    public class GetTiresBySizeAndType : IQuery<IEnumerable<TireDto>>
    {
        public string Size { get; set; }

        public TireType TireType { get; set; }
    }
}
