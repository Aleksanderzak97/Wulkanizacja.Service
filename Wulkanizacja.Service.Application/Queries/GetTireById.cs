using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
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
        [FromQuery(Name = "TireId")]
        public Guid TireId { get; set; }
    }
}
