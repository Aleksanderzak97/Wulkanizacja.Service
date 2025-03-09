using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Application.Dictionary
{
    public static class TranslationDictionary
    {
        private static readonly Dictionary<TireType, Dictionary<string, string>> Translations = new()
        {
            { TireType.Summer, new Dictionary<string, string> { { "pl", "Letnia" }, { "en", "Summer" } } },
            { TireType.Winter, new Dictionary<string, string> { { "pl", "Zimowa" }, { "en", "Winter" } } },
            { TireType.AllSeason, new Dictionary<string, string> { { "pl", "Całoroczna" }, { "en", "All Season" } } }
        };

        public static string Translate(TireType tireType, string languageCode)
        {
            if (Translations.TryGetValue(tireType, out var translations) && translations.TryGetValue(languageCode, out var translation))
            {
                return translation;
            }
            return tireType.ToString();
        }
    }
}
