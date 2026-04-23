using System.Windows;
using CurrencyAppWPF.ViewModels;

namespace CurrencyAppWPF
{
    public partial class AddCurrencyWindow : Window
    {
        private AddCurrencyViewModel _viewModel;

        public AddCurrencyWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _viewModel = new AddCurrencyViewModel(mainViewModel);
            _viewModel.RequestClose += (s, e) => this.Close();

            DataContext = _viewModel;
        }
    }
}