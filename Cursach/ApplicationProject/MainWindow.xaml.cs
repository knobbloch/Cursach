using System;
using System.Windows;
using System.Windows.Controls;
using ApplicationProject.Views;
using System.Globalization;

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
            PresentedView?.OnCultureChanged(culture);
        }

        public bool Present(IBaseView view)
        {
            if(view == null)
                throw new ArgumentNullException(nameof(view));
            else if(!view.IsPresentable || !(view is UserControl))
                return false;

            PresentedView?.Hide();
            if(PresentedView is ISupportOverlay overlay)
            {
                overlay.ClearOverlay();
                overlay.Overlay = null;
            }

            PresentedView = view;
            ActiveView.Content = view as UserControl;

            PresentedView?.OnCultureChanged(CurrentCulture);
            PresentedView?.Show();
            if(PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            return true;
        }
    }
}
