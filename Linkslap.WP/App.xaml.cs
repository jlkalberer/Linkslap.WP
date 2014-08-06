namespace Linkslap.WP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Linkslap.WP.BackgroundTask;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Utils;
    using Linkslap.WP.ViewModels;
    using Linkslap.WP.Views;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        /// <summary>
        /// The transitions.
        /// </summary>
        private TransitionCollection transitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class. 
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            MappingSetup.Map();

        }

        /// <summary>
        /// The on share target activated.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            var shareOperation = args.ShareOperation;
            MappingSetup.Map();

            var rootFrame = new Frame();
            Window.Current.Content = rootFrame;
            Window.Current.Activate();

            rootFrame.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () => rootFrame.Navigate(typeof(ShareLink), shareOperation));
        }

        /// <summary>
        /// The on activated.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                var protocalArgs = (ProtocolActivatedEventArgs)args;
                var url = protocalArgs.Uri;
                var queryString = ParseQueryString(url.Query);

                if (queryString.ContainsKey("streamKey"))
                {
                    var subscriptionStore = new SubscriptionStore();
                    var subscription = await subscriptionStore.Add(queryString["streamKey"]);

                    var rootFrame = Window.Current.Content as Frame;
                    var page = rootFrame.Content as Page;
                    var subscriptionViewModel = Mapper.Map<Subscription, SubscriptionViewModel>(subscription);
                    
                    page.Frame.Navigate(typeof(ViewStream), subscriptionViewModel);
                }
            }

            base.OnActivated(args);
        }


        private static Dictionary<string, string> ParseQueryString(string uri)
        {
            var substring = uri.Substring(((uri.LastIndexOf('?') == -1) ? 0 : uri.LastIndexOf('?') + 1));
            var pairs = substring.Split('&');
            return pairs.Select(piece => piece.Split('=')).ToDictionary(pair => pair[0], pair => pair[1]);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Register all the tasks...
            RegisterTasks.Run();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame { CacheSize = 1 };

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrameFirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            if (!string.IsNullOrEmpty(e.Arguments))
            {
                var linkRepo = new NewSlapsStore();
                var link = linkRepo.Links.FirstOrDefault(l => l.Id == int.Parse(e.Arguments));

                var page = rootFrame.Content as Page;
                if (link != null && page != null)
                {
                    var linkViewModel = Mapper.Map<Link, LinkViewModel>(link);

                    var viewLinksViewModel = new ViewLinksViewModel(linkViewModel);
                    page.Frame.Navigate(typeof(View), viewLinksViewModel);
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }
        
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrameFirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;

            if (rootFrame == null)
            {
                return;
            }

            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrameFirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}