﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Exceptions
{
    class TireNotFoundException : InfrastructureException
    {
        public TireNotFoundException(string message) : base(message) { }
        public override string Code => "tire_not_found";

    }
}
