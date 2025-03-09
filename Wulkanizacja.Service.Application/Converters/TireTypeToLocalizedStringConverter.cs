using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wulkanizacja.Service.Application.Dictionary;
using Wulkanizacja.Service.Core.Enums;

namespace Wulkanizacja.Service.Application.Converters
{
    public class TireTypeToLocalizedStringConverter
    {
        public object Convert(object value)
        {
            if (value is TireType tireType)
            {
                string languageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return TranslationDictionary.Translate(tireType, languageCode);
            }
            return value;
        }
    }
}
