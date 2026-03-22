using Wulkanizacja.Service.Application.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Dto;

namespace Wulkanizacja.Service.Application.Queries
{
    public class GetTireById : IQuery<TireDto>
    {
        public Guid TireId { get; set; }
    }
}
