using System;

namespace ApplicationProjectViews
{
    /// <summary>
    /// A type for a thread-safe view update action
    /// </summary>
    /// <param name="targetView">The view of which the update is performed.
    /// Must be castable to the type of the view on which the update is executed.</param>
    public delegate void ViewUpdate(IBaseView targetView);

    public interface IBaseView
    {
        /// <summary>
        /// Should be called right before the view is displayed by a IViewPresenter to determine whether the view is ready to be presented.
        /// </summary>
        bool Show();

        /// <summary>
        /// Dispatch a thread-safe UI update
        /// </summary>
        /// <param name="action">The update to dispatch. </param>
        void DispatchUpdate(ViewUpdate action);

        /// <summary>
        /// Should be called right before the view's presentability is determined
        /// </summary>
        event EventHandler ShowPreview;
    }
}
