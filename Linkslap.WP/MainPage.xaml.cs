namespace Linkslap.WP
{
    using System.Threading.Tasks;

    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Utils;
    using Linkslap.WP.Views;

    using Windows.UI.Xaml;
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
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
            : this(new AccountStore())
        {
        }

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
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                Application.Current.Exit();
                return;
            }

            var task = this.accountStore.Get();
            task.ContinueWith(
                t =>
                    {
                        if (t.Status == TaskStatus.Faulted || !t.IsCompleted || t.Result == null)
                        {
                            this.Navigate<Login>();
                            return;
                        }

                        this.Navigate<Home>();
                    });
        }
    }
}
