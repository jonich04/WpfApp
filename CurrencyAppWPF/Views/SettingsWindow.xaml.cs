using CurrencyAppWPF.Services;
using CurrencyAppWPF.ViewModels;
using System.Windows;

namespace CurrencyAppWPF
{
    public partial class SettingsWindow : Window
    {
        private SettingsViewModel _viewModel;

        public SettingsWindow(MainViewModel model)
        {
            InitializeComponent();

            _viewModel = new SettingsViewModel();
            _viewModel.DataServiceChanged += (s, useSqlite) =>
            {
                IDataService newService = useSqlite ? new SqliteDataService() : new JsonDataService();
                model.ChangeDataService(newService);
            };
            _viewModel.RequestClose += (s, e) => this.Close();

            DataContext = _viewModel;
        }
    }
}