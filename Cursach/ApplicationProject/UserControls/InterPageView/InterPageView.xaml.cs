using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
    public partial class InterPageView : UserControl, IViewPresenter, IInterPageView, INotifyPropertyChanged, ISupportOverlay
    {
        public InterPageView()
        {
            InitializeComponent();

            BankAccounts = new ObservableCollection<BankAccountInfo>();
            BankAccountsDisplayer.ItemsSource = BankAccounts;

            BankAccounts.Add(new BankAccountInfo("Сбербанк: 1234 5678 9123 4567", "20", "EUR"));
            BankAccounts.Add(new BankAccountInfo("ВТБ: 1234 5678 9123 4567", "40", "EUR"));
        }

        void Click_ProfileButton(object sender, RoutedEventArgs e)
        {
            if(!e.Handled)
            {
                ProfileSelected?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        void Click_CategoryButton(object sender, RoutedEventArgs e)
        {
            if(!e.Handled)
            {
                if(sender == AnalysisButton)
                    CategorySelected?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.Analysis)); 
                else if(sender == PlanButton)
                    CategorySelected?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.Plan));
                if(sender == NewEntryButton)
                    CategorySelected?.Invoke(this, new CategorySelectedEventArgs(CategorySelectedEventArgs.CategoryType.NewEntry));

                e.Handled = true;
            }
        }

        void Selected_BankAccount(object sender, MouseButtonEventArgs e)
        {
            if(BankAccountsDisplayer.SelectedIndex >= 0 && BankAccountsDisplayer.SelectedIndex < BankAccounts.Count)
            {
                MessageBox.Show(BankAccounts[BankAccountsDisplayer.SelectedIndex].AccountName);

                BankAccountSelected?.Invoke(this, new BankAccountSelectedEventArgs(BankAccounts[BankAccountsDisplayer.SelectedIndex], BankAccountsDisplayer.SelectedIndex));
            }
        }

        #region IViewPresenter
        public IBaseView PresentedView { get; protected set; }

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
            ActivePageView.Content = view as UserControl;

            PresentedView?.Show();
            if(PresentedView is ISupportOverlay overlay2)
                overlay2.Overlay = Overlay;

            return true;
        }
        #endregion

        #region IBaseView
        public void Show()
        {
            Shown?.Invoke(this, EventArgs.Empty);
        }
        public void Hide()
        {
            Hidden?.Invoke(this, EventArgs.Empty);
        }

        public void OnCultureChanged(CultureInfo culture)
        {

            PresentedView?.OnCultureChanged(culture);
        }

        public bool IsPresentable
        {
            get
            {
                return AnalysisButtonName != null &&
                       PlanButtonName != null &&
                       AccountName != null;
            }
        }

        public event EventHandler Shown;
        public event EventHandler Hidden;

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IInterPageView
        public event EventHandler<CategorySelectedEventArgs> CategorySelected;
        public event EventHandler ProfileSelected;
        public event EventHandler<BankAccountSelectedEventArgs> BankAccountSelected;

        public IList<BankAccountInfo> BankAccounts { get; }

        public IViewPresenter PageViewPresenter { get => this; }

        private string m_AnalysisButtonName;
        public string AnalysisButtonName
        {
            get => m_AnalysisButtonName;
            set
            {
                m_AnalysisButtonName = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalysisButtonName)));
            }
        }
        public string AnalysisButtonSymbol
        {
            get => m_AnalysisButtonName.Substring(0, 1);
        }

        private string m_PlanButtonName;
        public string PlanButtonName
        {
            get => m_PlanButtonName;
            set
            {
                m_PlanButtonName = value ?? "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlanButtonName)));
            }
        }
        public string PlanButtonSymbol
        {
            get => m_PlanButtonName.Substring(0, 1);
        }

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
        #endregion

        #region ISupportOverlay
        public Overlay Overlay { get; set; }
        
        public void ClearOverlay() { }
        #endregion
    }
}
