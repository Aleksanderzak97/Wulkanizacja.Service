using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Api.Exceptions
{
    class EmptyPostDataException : ApiExceptions
    {
        public EmptyPostDataException(string message) : base(message) { }

        public override string Code => "empty_post_data";
    }
}
