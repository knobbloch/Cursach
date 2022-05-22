using System.Globalization;

namespace ApplicationProject.Views
{
    public interface IViewPresenter : ICultureDependentData
    {
        /// <summary>
        /// Switches currently displayed view to its argument.
        /// </summary>
        /// <param name="view">The view to switch to.</param>
        /// <returns>True if the view has been presented successfully, false otherwise.</returns>
        bool Present(IBaseView view);

        IBaseView PresentedView { get; }
    }
}
