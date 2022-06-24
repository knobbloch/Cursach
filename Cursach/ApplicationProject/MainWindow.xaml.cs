using System;
using System.Windows;
using System.Windows.Controls;
using ApplicationProject.UserControls;
using System.Globalization;

using ApplicationProjectViews;

namespace ApplicationProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewPresenter
    {
        public IBaseView PresentedView { get; protected set; }
        public Overlay Overlay { get; }

        protected CultureInfo CurrentCulture { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Overlay = new Overlay(OverlayLayer);
        }

        public void OnCultureChanged(CultureInfo culture)
        {
            CurrentCulture = culture;
            (PresentedView as ICultureDependentData)?.OnCultureChanged(culture);
        }

        public bool Present(IBaseView view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            else if (!(view is UserControl && view.Show()))
                return false;

            if (PresentedView is ISupportOverlay overlay)
            {
                overlay.ClearOverlay();
                overlay.Overlay = null;
            }

            PresentedView = view;
            ActiveView.Content = view as UserControl;

            if (PresentedView is ICultureDependentData cultureDependent)
                cultureDependent.OnCultureChanged(CurrentCulture);
            if (PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            PresentedView.OnShown();

            return true;
        }
    }
}
