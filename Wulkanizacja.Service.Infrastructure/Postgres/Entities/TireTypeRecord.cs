using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Entities
{
    public class TireTypeRecord
    {

        public short TireTypeId { get; set; } // 1 = Letnia, 2 = Zimowa, 3 = Całoroczna
        public string Name { get; set; } // Nazwa typu (np. "Winter", "Summer", "All-Season")
    }
}
