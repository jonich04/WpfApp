using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CurrencyAppWPF.Models;
using CurrencyAppWPF.Services;

namespace CurrencyAppWPF.ViewModels
{
    public class AddCurrencyViewModel : BaseViewModel
    {
        private readonly JsonDataService _dataService;
        private readonly MainViewModel _mainViewModel;
        private string _charCode;
        public string CharCode
        {
            get => _charCode;
            set => SetProperty(ref _charCode, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public ICommand AddCommand => new RelayCommand(AddCurrency);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public event EventHandler RequestClose;

        public AddCurrencyViewModel(MainViewModel mainViewModel)
        {
            _dataService = new JsonDataService();
            _mainViewModel = mainViewModel;
        }
        private async void AddCurrency()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CharCode))
                {
                    ErrorMessage = "Введите код валюты";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Name))
                {
                    ErrorMessage = "Введите название валюты";
                    return;
                }

                if (!double.TryParse(Value, out double value))
                {
                    ErrorMessage = "Введите корректный курс";
                    return;
                }

                Currency newCurrency = new Currency
                {
                    Id = Guid.NewGuid().ToString(),
                    CharCode = CharCode.ToUpper(),
                    NumCode = "999",
                    Nominal = 1,
                    Name = Name,
                    Value = value,
                    Previous = value,
                    IsUserAdded = true,
                    AddedDate = DateTime.Now
                };

                _mainViewModel.Currencies.Add(newCurrency);

                var list = new System.Collections.Generic.List<Currency>();
                foreach (var c in _mainViewModel.Currencies)
                    list.Add(c);

                await _dataService.SaveCurrenciesAsync(list);

                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }
        private void Cancel()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}