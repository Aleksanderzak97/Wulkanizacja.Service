using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Api.Exceptions
{
    class EmptyUpdateDataException : ApiExceptions
    {
        public EmptyUpdateDataException(string message) : base(message) { }

        public override string Code => "empty_update_data";
    }
}
