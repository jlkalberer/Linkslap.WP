namespace Linkslap.WP
{
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Notifications;
    using Linkslap.WP.Controls;
    using Linkslap.WP.Utils;
    using Linkslap.WP.Views;

    using Windows.Phone.UI.Input;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        /// <summary>
        /// The account store.
        /// </summary>
        private readonly IAccountStore accountStore;
        
        /// <summary>
        /// Initializes static members of the <see cref="MainPage"/> class. 
        /// </summary>
        static MainPage()
        {
            NotificationStore = new NotificationStore();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
            : this(new AccountStore())
        {
        }

        /// <summary>
        /// Gets the notification store.
        /// </summary>
        public static NotificationStore NotificationStore { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        /// <param name="accountStore">
        /// The account repository.
        /// </param>
        public MainPage(IAccountStore accountStore)
        {
            this.InitializeComponent();

            this.accountStore = accountStore;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var task = this.accountStore.Get();
            task.ContinueWith(
                t =>
                    {
                        if (!t.IsCompleted || t.Result == null)
                        {
                            this.Navigate<Login>();
                            return;
                        }

                        NotificationStore.Register();
                        this.Navigate<Home>();
                    });
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (!this.ScenarioFrame.CanGoBack)
            {
                return;
            }

            this.ScenarioFrame.GoBack();

            //Indicate the back button press is handled so the app does not exit
            e.Handled = true;
        }
    }
}
