using System.Windows;
using CurrencyAppWPF.ViewModels;

namespace CurrencyAppWPF
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void OpenAddCurrencyWindow(object sender, RoutedEventArgs e)
        {
            AddCurrencyWindow addWindow = new AddCurrencyWindow(_viewModel);
            addWindow.Owner = this;
            addWindow.ShowDialog();
        }

        private void OpenSettingsWindow(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
    }
}