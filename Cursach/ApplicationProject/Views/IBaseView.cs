using System.Globalization;

namespace ApplicationProject.Views
{
    /// <summary>
    /// A type for a thread-safe view update action
    /// </summary>
    /// <param name="targetView">The view of which the update is performed.
    /// Must be castable to the type of the view on which the update is executed.</param>
    public delegate void ViewUpdate(IBaseView targetView);

    public interface IBaseView : ICultureDependentData
    {
        /// <summary>
        /// Should be called right after the view is displayed by a IViewPresenter.
        /// </summary>
        void Show();

        /// <summary>
        /// Should be called right before the view stops being displayed by a IViewPresenter.
        /// </summary>
        void Hide();

        /// <summary>
        /// Returns true if this view is ready to be presented, false otherwise.
        /// </summary>
        bool IsPresentable { get; }

        /// <summary>
        /// Dispatch a thread-safe UI update
        /// </summary>
        /// <param name="action">The update to dispatch. </param>
        void DispatchUpdate(ViewUpdate action);
    }
}
