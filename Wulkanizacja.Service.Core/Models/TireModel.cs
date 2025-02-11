using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Core.Models
{
    public class TireModel
    {
        public TireType TireType { get; set; }  // ID typu opony (np. letnia, zimowa)
        public string Id { get; set; }  // Unikalne ID opony
        public string ShortSerialNumber { get; set; }  // Skrócony numer seryjny
        public string Brand { get; init; }  // Marka opony (np. Michelin, Pirelli)
        public string Model { get; init; }  // Model opony (np. Pilot Sport 4)
        public string Size { get; init; }  // Rozmiar opony (np. 205/55 R16)
        public string SpeedIndex { get; init; }  // Indeks prędkości (np. W, H, T)
        public string LoadIndex { get; init; }  // Indeks nośności (np. 91)
        public DateTimeOffset? ManufactureDate { get; set; }  // Data produkcji opony
        public DateTimeOffset? CreateDate { get; set; }  // Data dodania do magazynu
        public DateTimeOffset? EditDate { get; set; }  // Data ostatniej edycji
        public string? Comments { get; set; }  // Dodatkowe uwagi    }
    }
}
