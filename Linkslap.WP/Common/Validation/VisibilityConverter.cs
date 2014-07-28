namespace Linkslap.WP.Common.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// The visibility converter.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IValidationMessage)
            {
                var message = value as IValidationMessage;
                return string.IsNullOrEmpty(message.Message) ? Visibility.Collapsed : Visibility.Visible;
            }
            
            if (value is IEnumerable<IValidationMessage>)
            {
                var messages = value as IEnumerable<IValidationMessage>;

                return messages.Any() ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
