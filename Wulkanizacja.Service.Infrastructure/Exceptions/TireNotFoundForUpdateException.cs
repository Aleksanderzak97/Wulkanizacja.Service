using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Exceptions
{
    public class TireNotFoundForUpdateException : InfrastructureException
    {
        public TireNotFoundForUpdateException(string message) : base(message) { }

        public override string Code => "update_tire_does_not_exist";
    }
}
