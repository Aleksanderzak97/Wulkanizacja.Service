using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Exceptions
{
    public class TireNotFoundForDeleteException : InfrastructureException
    {
        public TireNotFoundForDeleteException(string message) : base(message) { }

        public override string Code => "delete_tire_does_not_exist";
    }
}
