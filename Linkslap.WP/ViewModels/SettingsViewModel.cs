namespace Linkslap.WP.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Windows.UI.Popups;

    using Linkslap.WP.Common;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;
    using Linkslap.WP.Communication.Models;
    using Linkslap.WP.Communication.Util;

    /// <summary>
    /// The settings view model.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// The settings store.
        /// </summary>
        private readonly ISettingsStore settingsStore;

        private readonly ISubscriptionStore subscriptionStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
            : this(new SettingsStore(), new SubscriptionStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="settingsStore">
        /// The settings store.
        /// </param>
        private SettingsViewModel(ISettingsStore settingsStore, ISubscriptionStore subscriptionStore)
        {
            this.settingsStore = settingsStore;
            this.subscriptionStore = subscriptionStore;

            var settings = this.settingsStore.SubscriptionSettings;

            this.SubscriptionSettings =
                new ObservableCollection<SubscriptionSettingsViewModel>(
                    settings.Select(s => new SubscriptionSettingsViewModel(s, this.settingsStore)));

            this.DeleteSubscription = new RelayCommand(async model =>
                    {
                        var dialog = new MessageDialog("Do you really want to remove this subscription?");
                        dialog.Commands.Add(
                            new UICommand(
                                "Yes",
                                command =>
                                    {
                                        var setting = model as SubscriptionSettingsViewModel;

                                        if (setting == null)
                                        {
                                            return;
                                        }

                                        this.SubscriptionSettings.Remove(s => s.Id == setting.Id);

                                        this.subscriptionStore.Delete(setting.StreamKey);
                                        this.settingsStore.RemoveSubscriptionSetting(setting.Id);
                                    }));
                        dialog.Commands.Add(new UICommand("No"));
                        
                        await dialog.ShowAsync();
                    });
        }

        /// <summary>
        /// Gets or sets the delete item.
        /// </summary>
        public RelayCommand DeleteSubscription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether disable all notifications.
        /// </summary>
        public bool EnableAllNotifications
        {
            get
            {
                return !this.settingsStore.DisableAllNotifications;
            }

            set
            {
                this.settingsStore.DisableAllNotifications = !value;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the subscription settings.
        /// </summary>
        public ObservableCollection<SubscriptionSettingsViewModel> SubscriptionSettings { get; set; }

        /// <summary>
        /// The stream setting.
        /// </summary>
        public class SubscriptionSettingsViewModel : ViewModelBase
        {
            /// <summary>
            /// The settings.
            /// </summary>
            private readonly SubscriptionSettings settings;

            /// <summary>
            /// The settings store.
            /// </summary>
            private readonly ISettingsStore settingsStore;

            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionSettingsViewModel"/> class.
            /// </summary>
            /// <param name="settings">
            /// The settings.
            /// </param>
            /// <param name="settingsStore">
            /// The settings store.
            /// </param>
            public SubscriptionSettingsViewModel(SubscriptionSettings settings, ISettingsStore settingsStore)
            {
                this.settings = settings;
                this.settingsStore = settingsStore;
            }

            /// <summary>
            /// Gets the stream key.
            /// </summary>
            public int Id
            {
                get
                {
                    return this.settings.Id;
                }
            }

            /// <summary>
            /// Gets the stream key.
            /// </summary>
            public string StreamKey
            {
                get
                {
                    return this.settings.StreamKey;
                }
            }

            /// <summary>
            /// Gets or sets the stream name.
            /// </summary>
            public string StreamName
            {
                get
                {
                    return this.settings.StreamName;
                }

                set
                {
                    this.settings.StreamName = value;
                    this.settingsStore.SaveSubscriptionSettings();

                    this.OnPropertyChanged();
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether toast notifications.
            /// </summary>
            public bool ToastNotifications
            {
                get
                {
                    return this.settings.ToastNotifications;
                }

                set
                {
                    this.settings.ToastNotifications = value;
                    this.settingsStore.SaveSubscriptionSettings();

                    this.OnPropertyChanged();
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether show new links.
            /// </summary>
            public bool ShowNewLinks
            {
                get
                {
                    return this.settings.ShowNewLinks;
                }

                set
                {
                    this.settings.ShowNewLinks = value;
                    this.settingsStore.SaveSubscriptionSettings();

                    this.OnPropertyChanged();
                }
            }
        }
    }
}