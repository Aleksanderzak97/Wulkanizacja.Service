﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wulkanizacja.Service.Core.Events
{
    public record DeleteTireEvent(Guid TireId) : IDomainEvent;
}
