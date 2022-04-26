using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProject.Views
{
    public interface ISupportOverlay
    {
        /// <summary>
        /// Stores current overlay manager
        /// </summary>
        public Overlay Overlay { set; }

        /// <summary>
        /// Removes all items added by this class from the overlay
        /// </summary>
        public void ClearOverlay();
    }
}
