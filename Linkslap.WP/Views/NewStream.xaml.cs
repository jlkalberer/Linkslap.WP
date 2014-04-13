namespace Linkslap.WP.Views
{
    using System.Windows;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;

    using Microsoft.Phone.Controls;

    /// <summary>
    /// The new stream.
    /// </summary>
    public partial class NewStream : PhoneApplicationPage
    {
        /// <summary>
        /// The stream repository.
        /// </summary>
        private readonly IStreamRepository streamRepository;

        /// <summary>
        /// The view model.
        /// </summary>
        private readonly NewStreamViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewStream"/> class.
        /// </summary>
        public NewStream()
            : this(new StreamRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewStream"/> class.
        /// </summary>
        /// <param name="streamRepository">
        /// The stream repository.
        /// </param>
        public NewStream(IStreamRepository streamRepository)
        {
            this.streamRepository = streamRepository;
            this.InitializeComponent();

            this.viewModel = new NewStreamViewModel();
            this.viewModel.Busy = true;

            this.DataContext = this.viewModel;
        }

        /// <summary>
        /// The new stream click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void NewStream_Click(object sender, RoutedEventArgs eventArgs)
        {
            if (string.IsNullOrWhiteSpace(this.StreamName.Text))
            {
                return;
            }

            this.NewStreamButton.IsEnabled = false;
            this.StreamName.IsEnabled = false;
            this.viewModel.Busy = true;

            var task = this.streamRepository.NewStream(this.StreamName.Text);

            task.ContinueWith(
                stream =>
                    {
                        var bleh = 0;
                        this.Navigate("/Views/Home.xaml");
                    });
        }
    }
}