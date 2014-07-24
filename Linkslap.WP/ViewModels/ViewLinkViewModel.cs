using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linkslap.WP.ViewModels
{
    /// <summary>
    /// The view link view model.
    /// </summary>
    public class ViewLinkViewModel
    {
        public string Comment { get; set; }
        public string Info { get; set; }

        public string Url { get; set; }

        public bool CanGoBack { get; set; }
        public bool CanGoForward { get; set; }
    }
}
