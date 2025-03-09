using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Api.Exceptions;

namespace Wulkanizacja.Service.Infrastructure.Exceptions
{
    public class InvalidTireTypeException : ApiExceptions
    {
        public InvalidTireTypeException(string message) : base(message) { }

        public override string Code => "invalid_tire_type";
    }
}
