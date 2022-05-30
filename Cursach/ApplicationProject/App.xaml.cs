using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ApplicationProject.Views;
using ApplicationProject.Views.AnalysisPageView;

using ApplicationProject.UserControls.AnalysisPageView;

namespace ApplicationProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ViewsFlowController ViewsFlowController { get; }

        public App()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CurrentUICulture;

            ViewsFlowController = new ViewsFlowController((IViewPresenter)(MainWindow = new MainWindow()));

            ViewsFlowController.IInterPageViewInstance.AccountName = "Тест";
            ViewsFlowController.IDatedPageViewInstance.DateRangeTypes.Add(new Views.DatedPageView.DateRangeType()
            {
                Type = Views.DatedPageView.DateRangeType.RangeType.MONTH
            });

            _ = ViewsFlowController.Link();
            MainWindow.Show();
        }
    }
}
