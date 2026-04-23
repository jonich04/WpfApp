using CurrencyAppWPF.Services;
using CurrencyAppWPF.ViewModels;
using System.Windows;

namespace CurrencyAppWPF
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            IDataService dataService = GetDataServiceFromSettings();
            _viewModel = new MainViewModel(dataService);
            DataContext = _viewModel;
        }
                private IDataService GetDataServiceFromSettings()
        {
            string settingsPath = System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                "CurrencyAppWPF",
                "settings.txt");
            
            if (System.IO.File.Exists(settingsPath))
            {
                string setting = System.IO.File.ReadAllText(settingsPath);
                if (setting == "SQLITE")
                {
                    return new SqliteDataService();
                }
            }
            
            return new JsonDataService();
        }

        private void OpenAddCurrencyWindow(object sender, RoutedEventArgs e)
        {
            AddCurrencyWindow addWindow = new AddCurrencyWindow(_viewModel);
            addWindow.Owner = this;
            addWindow.ShowDialog();
        }

        private void OpenSettingsWindow(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(_viewModel);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
    }
}