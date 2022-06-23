using System;
using System.Globalization;
using System.ComponentModel;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

using ApplicationProjectViews;
using ApplicationProjectViews.AddIncomeCategoryPageView;

namespace ApplicationProject.UserControls.AddIncomeCategoryPageView
{
    /// <summary>
    /// Interaction logic for AddExpensePageView.xaml
    /// </summary>
    public partial class AddIncomeCategoryPageView : UserControl, IAddIncomeCategoryPageView, INotifyPropertyChanged, ICultureDependentData
    {
        protected const string CategoryNameFieldTextKey = "PAGE_ADDINCOMECATEGORY_NAMEFIELD_NAME";
        protected const string CurrencyAmountFieldTextKey = "PAGE_ADDINCOMECATEGORY_CURRENCYAMOUNTFIELD_NAME";
        protected const string CategoryImagePathFieldTextKey = "PAGE_ADDINCOMECATEGORY_CATEGORYIMAGEPATH_NAME";
        protected const string ButtonAddTextKey = "PAGE_ADDINCOMECATEGORY_BUTTONADD_NAME";
        protected const string ButtonExitTextKey = "PAGE_ADDINCOMECATEGORY_BUTTONEXIT_NAME";
        protected const string ImageSelectorTextKey = "PAGE_ADDINCOMECATEGORY_IMAGESELECTOR_NAME";

        public AddIncomeCategoryPageView()
        {
            m_CategoryName = "";
            m_CategoryImagePath = "";
            m_CurrencyAmount = 0;
            CurrentCulture = null;

            InitializeComponent();
        }

        public bool IsValid
        {
            get
            {
                return CategoryNameError == null &&
                       CurrencyAmountError == null &&
                       CategoryImagePathError == null &&
                       !Validation.GetHasError(CurrencyAmountBox) &&
                       !Validation.GetHasError(CategoryNameBox) &&
                       !Validation.GetHasError(CategoryImagePathBox);
            }
        }

        protected CultureInfo CurrentCulture
        {
            get => m_CurrentCulture;
            set
            {
                m_CurrentCulture = value ?? System.Threading.Thread.CurrentThread.CurrentUICulture ?? CultureInfo.CurrentUICulture ?? CultureInfo.CurrentCulture ?? CultureInfo.InvariantCulture;

                RefreshLocalization();
            }
        }
        private CultureInfo m_CurrentCulture;

        public string CategoryNameFieldText => GetLocalizedString(CategoryNameFieldTextKey);
        public string CurrencyAmountFieldText => GetLocalizedString(CurrencyAmountFieldTextKey);
        public string CategoryImagePathFieldText => GetLocalizedString(CategoryImagePathFieldTextKey);
        public string ButtonAddText => GetLocalizedString(ButtonAddTextKey);
        public string ButtonExitText => GetLocalizedString(ButtonExitTextKey);

        #region IBaseView
        public bool Show()
        {
            ShowPreview?.Invoke(this, EventArgs.Empty);

            return true;
        }


        public void OnCultureChanged(CultureInfo newCulture)
        {
            CurrentCulture = newCulture;
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

        #region IAddExpenseCategoryPageView
        public event EventHandler AddAction;
        public event EventHandler AddActionPost;
        public event EventHandler ExitAction;

        public string CategoryName
        {
            get => m_CategoryName;
            set
            {
                m_CategoryName = value ?? throw new ArgumentNullException(nameof(CategoryName));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));
            }
        }
        private string m_CategoryName;

        public ValueInputError CategoryNameError
        {
            get => m_CategoryNameError;
            set
            {
                m_CategoryNameError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryNameError)));
            }
        }
        private ValueInputError m_CategoryNameError;

        public decimal CurrencyAmount
        {
            get => m_CurrencyAmount;
            set
            {
                m_CurrencyAmount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmount)));
            }
        }
        private decimal m_CurrencyAmount;

        public ValueInputError CurrencyAmountError
        {
            set
            {
                m_CurrencyAmountError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmountError)));
            }
            get => m_CurrencyAmountError;
        }
        private ValueInputError m_CurrencyAmountError;

        public string CategoryImagePath
        {
            get => m_CategoryImagePath;
            set
            {
                m_CategoryImagePath = value ?? throw new ArgumentNullException(nameof(CategoryImagePath));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryImagePath)));
            }
        }
        private string m_CategoryImagePath;


        public ValueInputError CategoryImagePathError
        {
            get => m_CategoryImagePathError;
            set
            {
                m_CategoryImagePathError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryImagePathError)));
            }
        }
        private ValueInputError m_CategoryImagePathError;
        #endregion

        #region Methods
        public void RefreshLocalization()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryNameFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrencyAmountFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryImagePathFieldText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonAddText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonExitText)));
        }

        private string GetLocalizedString(string key)
        {
            return ApplicationProject.Resources.Locale.ResourceManager.GetString(key, CurrentCulture);
        }
        #endregion

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAction?.Invoke(this, EventArgs.Empty);
            AddActionPost?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            ExitAction?.Invoke(this, EventArgs.Empty);
        }

        private void ImagePathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog imageSelector = new();
            imageSelector.Filter = "Image Files (*.BMP;*.JPG;*.PNG;*.GIF)|*.BMP;*.JPG;*.PNG;*.GIF";

            if (File.Exists(CategoryImagePath))
                imageSelector.Filter = Path.GetDirectoryName(Path.GetFullPath(CategoryImagePath));

            imageSelector.Title = GetLocalizedString(ImageSelectorTextKey);

            if (imageSelector.ShowDialog() == true)
                CategoryImagePath = imageSelector.FileName;
        }
    }
}
