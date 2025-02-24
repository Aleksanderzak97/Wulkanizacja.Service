using Convey.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Application.Mapping;
using Wulkanizacja.Service.Core.Repositories;

namespace Wulkanizacja.Service.Application.Queries.Handlers
{
    public class GetTireByIdHandler(ITiresRepository tiresRepository) : IQueryHandler<GetTireById, TireDto>
    {
        public async Task<TireDto> HandleAsync(GetTireById query, CancellationToken cancellationToken = default)
        {
            var tires = await tiresRepository.GetByIdAsync(query.TireId, cancellationToken);
            return await Task.FromResult(tires.ToDto());
        }
    }
}
