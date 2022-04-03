﻿using System;
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

using ApplicationProject.Views.InterPageView;
using ApplicationProject.Views;

namespace ApplicationProject.UserControls.InterPageView
{
    /// <summary>
    /// Interaction logic for InterPageView.xaml
    /// </summary>
    public partial class InterPageView : UserControl, IViewPresenter, IInterPageView
    {
        public InterPageView()
        {
            InitializeComponent();
            
            BankAccounts = new ObservableCollection<BankAccountInfo>();
            BankAccountsDisplayer.ItemsSource = BankAccounts;

            BankAccounts.Add(new BankAccountInfo("Сбербанк: 1234 5678 9123 4567", "20 EUR"));
            BankAccounts.Add(new BankAccountInfo("ВТБ: 1234 5678 9123 4567", "40 RUB"));
        }

        void Click_ProfileButton(object sender, RoutedEventArgs e)
        {
            if(!e.Handled)
            {
                ProfileSelected?.Invoke(this, new EventArgs());
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
            MessageBox.Show(BankAccounts[BankAccountsDisplayer.SelectedIndex].AccountName);

            BankAccountSelected?.Invoke(this, new BankAccountSelectedEventArgs(BankAccounts[BankAccountsDisplayer.SelectedIndex], BankAccountsDisplayer.SelectedIndex));
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
            PresentedView = view;
            //ActivePageView = view as UserControl;
            PresentedView?.Show();

            return true;
        }
        #endregion

        #region IBaseView
        public void Show() {}
        public void Hide() {}
        public bool IsPresentable { get; } = true;

        public event EventHandler Shown;
        public event EventHandler Hidden;

        #endregion

        #region IInterPageView
        public event EventHandler<CategorySelectedEventArgs> CategorySelected;
        public event EventHandler ProfileSelected;
        public event EventHandler<BankAccountSelectedEventArgs> BankAccountSelected;

        public IList<BankAccountInfo> BankAccounts { get; }

        public IViewPresenter PageViewPresenter { get => this; }
        #endregion
    }
}
