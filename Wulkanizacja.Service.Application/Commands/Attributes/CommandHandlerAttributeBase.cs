using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Application.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandHandlerAttributeBase : Attribute
    {
    }
}
