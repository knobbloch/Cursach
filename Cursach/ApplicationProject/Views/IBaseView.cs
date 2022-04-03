using System;

namespace ApplicationProject.Views
{
    public interface IBaseView
    {
        /// <summary>
        /// Called right after the view is displayed.
        /// </summary>
        void Show();

        /// <summary>
        /// Called right before the view stops being displayed.
        /// </summary>
        void Hide();

        /// <summary>
        /// Returns true if this view is ready to be presented, false otherwise.
        /// </summary>
        bool IsPresentable { get; }

        event EventHandler Shown;
        event EventHandler Hidden;
    }
}
