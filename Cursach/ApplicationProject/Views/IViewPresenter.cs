using System.Globalization;

namespace ApplicationProject.Views
{
    public interface IViewPresenter
    {
        /// <summary>
        /// Switches currently displayed view to its argument.
        /// </summary>
        /// <param name="view">The view to switch to.</param>
        /// <returns>True if the view has been presented successfully, false otherwise.</returns>
        bool Present(IBaseView view);

        /// <summary>
        /// Is called when the current UI culture changes
        /// </summary>
        /// <param name="culture">The new UI culture</param>
        void OnCultureChanged(CultureInfo culture);

        IBaseView PresentedView { get; }
    }
}
