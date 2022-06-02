using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ApplicationProjectViews;
using ApplicationProjectViews.AnalysisPageView;

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
            //System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            ViewsFlowController = new ViewsFlowController((IViewPresenter)(MainWindow = new MainWindow()));

            ViewsFlowController.IInterPageViewInstance.AccountName = "Тест";
            ViewsFlowController.IDatedPageViewInstance.DateRangeTypes.Add(ApplicationProjectViews.DatedPageView.DateRangeType.MONTH);

            _ = ViewsFlowController.Link();
            MainWindow.Show();
        }
    }
}
