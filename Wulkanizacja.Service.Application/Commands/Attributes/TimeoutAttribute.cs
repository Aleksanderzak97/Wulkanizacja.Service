using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Application.Commands.Attributes
{
    public class TimeoutAttribute : HandlerAttributeBase
    {
        #region Properties

        public TimeSpan Timeout { get; set; }

        #endregion

        #region Constructors

        public TimeoutAttribute(TimeSpan timeOut)
        {
            Timeout = timeOut;
        }

        #endregion
    }
}
