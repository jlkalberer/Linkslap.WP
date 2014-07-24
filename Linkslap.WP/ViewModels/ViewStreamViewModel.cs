namespace Linkslap.WP.ViewModels
{
    using System.Collections.ObjectModel;

    public class ViewStreamViewModel : ViewModelBase
    {
        public ViewStreamViewModel()
        {
            this.Links = new ObservableCollection<LinkViewModel>();
        }

        public string StreamName { get; set; }

        public ObservableCollection<LinkViewModel> Links { get; private set; }
    }
}
