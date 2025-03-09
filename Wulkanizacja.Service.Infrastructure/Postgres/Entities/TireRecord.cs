using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Infrastructure.Postgres.Entities
{
    public class TireRecord
    {
        [Key]
        public Guid TireId { get; init; } // Unikalne ID opony

        [Required]
        public string Brand { get; init; } // Marka opony (np. Michelin, Pirelli)

        [Required]
        public string Model { get; init; } // Model opony

        [Required]
        public string Size { get; init; } // Rozmiar (np. 205/55 R16)

        [Required]
        public short TireTypeId { get; set; } // 1 = Letnia, 2 = Zimowa, 3 = Całoroczna

        [Required]
        public string SpeedIndex { get; set; } // Indeks prędkości (np. Y)

        [Required]
        public string LoadIndex { get; set; } // Indeks nośności (np. 91)

        [Required]
        public string ManufactureDate { get; set; } // Data produkcji

        [Column(TypeName = "timestamp with time zone")]
        public DateTimeOffset? CreationDate { get; set; } // Data utworzenia w systemie

        [Column(TypeName = "timestamp with time zone")]
        public DateTimeOffset? EditDate { get; set; } // Data ostatniej edycji

        public string? Comments { get; init; } // Opcjonalne uwagi o oponie

        [Required]
        public int QuantityInStock { get; set; } // Ilość sztuk na stanie

        public TireTypeRecord TireType { get; set; } // Relacja do tabeli typów opon
    }
}


