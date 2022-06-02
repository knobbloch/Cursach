using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

using ApplicationProject.Views.InterPageView;
using ApplicationProject.Views;

namespace ApplicationProject.UserControls.InterPageView
{
    /// <summary>
    /// Interaction logic for InterPageView.xaml
    /// </summary>
    public partial class InterPageView : UserControl, IInterPageView, INotifyPropertyChanged, ISupportOverlay, IViewPresenter
    {
        protected const string AnalysisButtonNameKey = "PAGE_ANALYSIS_BUTTON_ANALYSIS";
        protected const string PlanButtonNameKey = "PAGE_ANALYSIS_BUTTON_PLAN";

        public InterPageView()
        {
            InitializeComponent();

            BankAccountsDisplayer.ItemsSource = BankAccounts = new ObservableCollection<BankAccountInfo>();

            CurrentCulture = null;
        }

        private CultureInfo m_CurrentCulture;
        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture ?? CultureInfo.InvariantCulture;
                RefreshLocalization();
            }
        }

        #region IViewPresenter
        public IBaseView PresentedView { get; private set; }

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
            ActivePageView.Content = view as UserControl;

            PresentedView?.OnCultureChanged(CurrentCulture);
            if (PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            return true;
        }
        #endregion

        #region IBaseView
        public bool Show()
        {
            ShowPreview?.Invoke(this, EventArgs.Empty);

            return AnalysisButtonName.Length > 0 &&
                   PlanButtonName.Length > 0 &&
                   AccountName?.Length > 0;
        }

        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;
            PresentedView?.OnCultureChanged(newCulture);
        }

        public void DispatchUpdate(ViewUpdate action)
        {
            Dispatcher.Invoke(() => action(this));
        }

        public event EventHandler ShowPreview;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IInterPageView
        public string AnalysisButtonName => GetLocalizedString(AnalysisButtonNameKey);
        public string AnalysisButtonSymbol => AnalysisButtonName.Substring(0, 1);

        public string PlanButtonName => GetLocalizedString(PlanButtonNameKey);
        public string PlanButtonSymbol => PlanButtonName.Substring(0, 1);

        private string m_AccountName;
        public string AccountName
        {
            get => m_AccountName;
            set
            {
                m_AccountName = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountName)));
            }
        }

        public event CategorySelectedEventHandler CategorySelectedAction;
        public event EventHandler ProfileSelectedAction;
        public event BankAccountSelectedSelectedEventHandler BankAccountSelected;

        public IList<BankAccountInfo> BankAccounts { get; }
        #endregion

        #region ISupportOverlay
        public Overlay Overlay { get; set; }

        public void ClearOverlay() { }
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalysisButtonName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalysisButtonSymbol)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlanButtonName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlanButtonSymbol)));
        }

        private string GetLocalizedString(string key)
        {
            return ApplicationProject.Resources.Locale.ResourceManager.GetString(key, CurrentCulture);
        }
        #endregion

        #region Handled Events
        private void Click_ProfileButton(object sender, RoutedEventArgs e)
        {
            if (!e.Handled)
            {
                ProfileSelectedAction?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void Click_CategoryButton(object sender, RoutedEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender == AnalysisButton)
                    CategorySelectedAction?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.Analysis));
                else if (sender == PlanButton)
                    CategorySelectedAction?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.Plan));
                else if (sender == NewEntryButton)
                    CategorySelectedAction?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.NewEntry));

                e.Handled = true;
            }
        }

        private void Selected_BankAccount(object sender, MouseButtonEventArgs e)
        {
            if (BankAccountsDisplayer.SelectedIndex >= 0 && BankAccountsDisplayer.SelectedIndex < BankAccounts.Count)
                BankAccountSelected?.Invoke(this, new BankAccountSelectedEventArgs(BankAccounts[BankAccountsDisplayer.SelectedIndex], BankAccountsDisplayer.SelectedIndex));
        }
        #endregion
    }
}
