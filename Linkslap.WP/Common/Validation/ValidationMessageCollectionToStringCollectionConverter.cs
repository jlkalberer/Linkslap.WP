using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linkslap.WP.Common.Validation
{
    using Windows.UI.Xaml.Data;

    class ValidationMessageCollectionToStringCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is IEnumerable<IValidationMessage>))
            {
                return new List<string>();
            }

            List<string> lst = (value as IEnumerable<IValidationMessage>).Select(m => m.Message).ToList();
            return lst;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
