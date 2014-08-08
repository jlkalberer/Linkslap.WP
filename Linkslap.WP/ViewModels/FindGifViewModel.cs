namespace Linkslap.WP.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;

    using Windows.Foundation;
    using Windows.UI.Xaml.Data;

    using AutoMapper;

    using Linkslap.WP.Common.Validation;
    using Linkslap.WP.Communication;
    using Linkslap.WP.Communication.Interfaces;

    /// <summary>
    /// The find gif view model.
    /// </summary>
    public class FindGifViewModel : ValidationBase
    {
        /// <summary>
        /// The gif store.
        /// </summary>
        private readonly IGifStore gifStore;

        private bool isSearching;

        private bool executeButtonEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindGifViewModel"/> class.
        /// </summary>
        public FindGifViewModel()
            : this(new GifStore())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindGifViewModel"/> class.
        /// </summary>
        /// <param name="gifStore">
        /// The gif Store.
        /// </param>
        public FindGifViewModel(IGifStore gifStore)
        {
            this.gifStore = gifStore;
            this.ExecuteButtonEnabled = true;

            this.Results = new LazyLoadedCollection(this);
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NSFW.
        /// </summary>
        public bool Nsfw { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether execute button enabled.
        /// </summary>
        public bool ExecuteButtonEnabled
        {
            get
            {
                return this.executeButtonEnabled;
            }
            set
            {
                this.executeButtonEnabled = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is searching.
        /// </summary>
        public bool IsSearching
        {
            get
            {
                return this.isSearching;
            }
            set
            {
                this.isSearching = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        public LazyLoadedCollection Results { get; private set; }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return this.ExecuteButtonEnabled;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public async override void Execute(object parameter)
        {
            this.IsSearching = true;
            this.ExecuteButtonEnabled = false;
            base.Execute(parameter);

            if (this.HasErrors)
            {
                this.ExecuteButtonEnabled = true;
                this.OnPropertyChanged(string.Empty);
                return;
            }

            this.Results.Clear();
            await this.Results.Search(this.Query);

            this.ExecuteButtonEnabled = true;
            this.OnPropertyChanged(string.Empty);
            this.IsSearching = false;
        }

        /// <summary>
        /// The lazy loaded collection.
        /// </summary>
        public class LazyLoadedCollection : ObservableCollection<GifViewModel>, ISupportIncrementalLoading
        {
            private const int PageSize = 20;

            /// <summary>
            /// The gif view model.
            /// </summary>
            private readonly FindGifViewModel gifViewModel;

            private int page;

            /// <summary>
            /// Initializes a new instance of the <see cref="LazyLoadedCollection"/> class.
            /// </summary>
            /// <param name="gifViewModel">
            /// The gif view model.
            /// </param>
            public LazyLoadedCollection(FindGifViewModel gifViewModel)
            {
                this.gifViewModel = gifViewModel;
            }

            /// <summary>
            /// The search.
            /// </summary>
            /// <param name="query">
            /// The query.
            /// </param>
            /// <param name="page">
            /// The page.
            /// </param>
            public async Task<GifMeModel> Search(string query)
            {
                this.page = 0;
                this.HasMoreItems = false;
                var result = this.gifViewModel.gifStore.Search(query, this.gifViewModel.Nsfw, page);
                await result.ContinueWith(this.gifViewModel.ValidateResponse);

                if (result.Result.Status != 200)
                {
                    return null;
                }

                this.Clear();
                result.Result.Results.Each(i => this.Add(new GifViewModel { Gif = i.Link, Thumbnail = i.Thumb }));

                this.HasMoreItems = result.Result.Meta.Total > PageSize * (page + 1);
                return result.Result;
            }

            public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
            {
                return AsyncInfo.Run(
                    ct => this.LoadMore());
            }

            public bool HasMoreItems { get; private set; }

            private async Task<LoadMoreItemsResult> LoadMore()
            {
                this.page += 1;
                var result = this.gifViewModel.gifStore.Search(this.gifViewModel.Query, this.gifViewModel.Nsfw, this.page);
                await result.ContinueWith(this.gifViewModel.ValidateResponse);

                if (result.Result.Status != 200)
                {
                    return new LoadMoreItemsResult();
                }

                result.Result.Results.Each(i => this.Add(new GifViewModel { Gif = i.Link, Thumbnail = i.Thumb }));

                this.HasMoreItems = result.Result.Meta.Total > PageSize * (page + 1);
                var meta = result.Result.Meta;
                return new LoadMoreItemsResult { Count = (uint)(meta.Total - ((meta.Page + 1) * meta.Limit)) };
            }
        }
    }
}