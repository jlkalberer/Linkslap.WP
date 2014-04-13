using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linkslap.WP.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Linkslap.WP.Annotations;

    public class NewStreamViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The busy.
        /// </summary>
        private bool busy;

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether busy.
        /// </summary>
        public bool Busy
        {
            get
            {
                return this.busy;
            }
            set
            {
                if (value.Equals(this.busy))
                {
                    return;
                }

                this.busy = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the stream name.
        /// </summary>
        public string StreamName { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
