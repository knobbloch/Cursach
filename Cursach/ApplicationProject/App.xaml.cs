using System.Windows;

using ApplicationProjectViews;
using WpfLibrary;

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
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CurrentUICulture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");

            ViewsFlowController = new ViewsFlowController((IViewPresenter)(MainWindow = new MainWindow()));

            ViewsFlowController.IInterPageViewInstance.AccountName = "Тест";
            ViewsFlowController.IDatedPageViewInstance.DateRangeTypes.Add(ApplicationProjectViews.DatedPageView.DateRangeType.MONTH);

            PrezenterInitializer test = new PrezenterInitializer(ViewsFlowController.IAnalysisPageViewInstance,
                                            ViewsFlowController.IDatedPageViewInstance,
                                            ViewsFlowController.IInterPageViewInstance,
                                            ViewsFlowController.IPlanPageViewInstance,

                                            ViewsFlowController.IAddBankAccountPageViewInstance,            //IAddBankAccountPageView
                                            ViewsFlowController.IAddExpenseCategoryPageViewInstance,        //IAddExpenseCategoryPageView
                                            ViewsFlowController.IAddExpensePageViewInstance,                //IAddExpensePageView
                                            ViewsFlowController.IAddIncomeCategoryPageViewInstance,         //IAddIncomeCategoryPageView
                                            ViewsFlowController.IAddIncomePageViewInstance);                //IAddIncomePageView

            _ = ViewsFlowController.Link();
            MainWindow.Show();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
