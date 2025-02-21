using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Dto;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Application.Commands
{
    public class PostTire : ICommand
    {
        public TireDto Tire { get; set; }
    }
}
