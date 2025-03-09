using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Api.Exceptions
{
    public class BadIdentifierException : ApiExceptions
    {
        public BadIdentifierException(string message) : base(message) { }
        public override string Code => "bad_identifier";
    }
}
