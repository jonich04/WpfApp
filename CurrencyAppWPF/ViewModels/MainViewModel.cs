using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CurrencyAppWPF.Models;
using CurrencyAppWPF.Services;

namespace CurrencyAppWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly CurrencyApiService _apiService;

        public ObservableCollection<Currency> Currencies { get; } = new();

        private string _lastSessionDate;
        public string LastSessionDate
        {
            get => _lastSessionDate;
            set => SetProperty(ref _lastSessionDate, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IAsyncRelayCommand LoadCurrenciesCommand { get; }
        public IAsyncRelayCommand UpdateFromApiCommand { get; }
        public IRelayCommand<Currency> DeleteCurrencyCommand { get; }

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _apiService = new CurrencyApiService();

            LoadCurrenciesCommand = new AsyncRelayCommand(LoadCurrencies);
            UpdateFromApiCommand = new AsyncRelayCommand(UpdateFromApi);
            DeleteCurrencyCommand = new RelayCommand<Currency>(DeleteCurrency);

            LoadLastSessionDate();
            _ = LoadCurrencies();
        }

        private async Task LoadCurrencies()
        {
            IsLoading = true;

            try
            {
                var savedCurrencies = await _dataService.LoadCurrenciesAsync();

                Currencies.Clear();

                if (savedCurrencies.Count > 0)
                {
                    foreach (var currency in savedCurrencies)
                        Currencies.Add(currency);
                }
                else
                {
                    await UpdateFromApi();
                }
            }
            catch (Exception ex)
            {
                LastSessionDate = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateFromApi()
        {
            IsLoading = true;

            try
            {
                var userCurrencies = new ObservableCollection<Currency>();
                foreach (var c in Currencies)
                {
                    if (c.IsUserAdded)
                        userCurrencies.Add(c);
                }

                var apiCurrencies = await _apiService.LoadCurrenciesFromApiAsync();

                Currencies.Clear();

                foreach (var currency in apiCurrencies)
                    Currencies.Add(currency);

                foreach (var userCurrency in userCurrencies)
                    Currencies.Add(userCurrency);

                await _dataService.SaveCurrenciesAsync(new System.Collections.Generic.List<Currency>(Currencies));

                SaveCurrentSessionDate();
            }
            catch (Exception ex)
            {
                LastSessionDate = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void DeleteCurrency(Currency currency)
        {
            if (currency == null) return;

            Currencies.Remove(currency);

            await _dataService.SaveCurrenciesAsync(
                new System.Collections.Generic.List<Currency>(Currencies));
        }

        private void LoadLastSessionDate()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CurrencyAppWPF");

            string sessionFile = Path.Combine(appDataPath, "session.txt");

            if (File.Exists(sessionFile))
            {
                string date = File.ReadAllText(sessionFile);
                LastSessionDate = $"Последняя сессия: {date}";
            }
            else
            {
                LastSessionDate = "Первый запуск";
            }
        }

        private void SaveCurrentSessionDate()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CurrencyAppWPF");

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            string sessionFile = Path.Combine(appDataPath, "session.txt");
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            File.WriteAllText(sessionFile, currentDate);
            LastSessionDate = $"Последняя сессия: {currentDate}";
        }
    }
}