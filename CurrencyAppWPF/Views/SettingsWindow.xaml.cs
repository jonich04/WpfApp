using System.Windows;
using CurrencyAppWPF.ViewModels;

namespace CurrencyAppWPF
{
    public partial class SettingsWindow : Window
    {
        private SettingsViewModel _viewModel;

        public SettingsWindow()
        {
            InitializeComponent();

            _viewModel = new SettingsViewModel();
            _viewModel.RequestClose += (s, e) => this.Close();

            DataContext = _viewModel;
        }
    }
}