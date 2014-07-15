namespace Linkslap.WP.Views
{
    using Windows.UI.Xaml;

    using Linkslap.WP.Controls;
    using Linkslap.WP.ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : PageBase
    {
        /// <summary>
        /// The view model.
        /// </summary>
        private readonly RegisterViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Register"/> class.
        /// </summary>
        public Register()
        {
            this.InitializeComponent();

            this.viewModel = new RegisterViewModel();
            this.DataContext = viewModel;
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!this.viewModel.Valid())
            {
                return;
            }

            // register stuff
        }
    }
}
