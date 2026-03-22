using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Application.Mapping;
using Wulkanizacja.Service.Core.Repositories;
using Wulkanizacja.Service.Application.CQRS.Queries;

namespace Wulkanizacja.Service.Application.Queries.Handlers
{
    public class GetTiresBySizeAndTypeHandler(ITiresRepository tiresRepository) : IQueryHandler<GetTiresBySizeAndType, IEnumerable<TireDto>>
    {

        public async Task<IEnumerable<TireDto>> HandleAsync(GetTiresBySizeAndType query, CancellationToken cancellationToken = default)
        {
            var tires = await tiresRepository.GetBySizeAndTypeAsync(query.Size, query.TireType, cancellationToken);
            return await Task.FromResult(tires.ToDto());
        }
    }
}
