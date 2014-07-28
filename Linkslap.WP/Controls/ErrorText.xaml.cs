namespace Linkslap.WP.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using Linkslap.WP.Common.Validation;

    public sealed partial class ErrorText : UserControl
    {
        public ErrorText()
        {
            this.InitializeComponent();

            var ctx = this.DataContext;
        }

        /// <summary>
        /// The links property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(IValidationMessage),
            typeof(ErrorText), 
            new PropertyMetadata(default(IValidationMessage), PropertyChangedCallback));

        /// <summary>
        /// The property changed callback.
        /// </summary>
        /// <param name="dependencyObject">
        /// The dependency object.
        /// </param>
        /// <param name="dependencyPropertyChangedEventArgs">
        /// The dependency property changed event args.
        /// </param>
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyPropertyChangedEventArgs.NewValue is IEnumerable<IValidationMessage>)
            {
                (dependencyObject as ErrorText).DataContext =
                    (dependencyPropertyChangedEventArgs.NewValue as IEnumerable<IValidationMessage>).FirstOrDefault();
            }
            else
            {
                (dependencyObject as ErrorText).DataContext = dependencyPropertyChangedEventArgs.NewValue as IValidationMessage;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether removable links.
        /// </summary>
        public IValidationMessage Text
        {
            get
            {
                return (IValidationMessage)GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }
    }
}
