using System;
using System.Globalization;

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
        /// Called when current culture (localization) changes
        /// </summary>
        /// <param name="culture">The new UI culture</param>
        void OnCultureChanged(CultureInfo culture);

        /// <summary>
        /// Returns true if this view is ready to be presented, false otherwise.
        /// </summary>
        bool IsPresentable { get; }

        /// <summary>
        /// Called when this view is shown
        /// </summary>
        event EventHandler Shown;

        /// <summary>
        /// Called when this view is no longer shown
        /// </summary>
        event EventHandler Hidden;
    }
}
