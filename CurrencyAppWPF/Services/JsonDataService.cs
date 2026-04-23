using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyAppWPF.Models;

namespace CurrencyAppWPF.Services
{
    public class JsonDataService : IDataService
    {
        private readonly string _filePath;

        public JsonDataService()
        {
            
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string appDataPath = System.IO.Path.Combine(desktopPath, "CurrencyAppWPF");

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            _filePath = System.IO.Path.Combine(appDataPath, "currencies.json");
        }

        public async Task SaveCurrenciesAsync(List<Currency> currencies)
        {
            string json = JsonSerializer.Serialize(currencies, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<List<Currency>> LoadCurrenciesAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Currency>();

            string json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Currency>>(json) ?? new List<Currency>();
        }

        public async Task AddCurrencyAsync(Currency currency)
        {
            var currencies = await LoadCurrenciesAsync();
            currencies.Add(currency);
            await SaveCurrenciesAsync(currencies);
        }

        public async Task DeleteCurrencyAsync(string charCode)
        {
            var currencies = await LoadCurrenciesAsync();
            var toRemove = currencies.Find(c => c.CharCode == charCode);
            if (toRemove != null)
            {
                currencies.Remove(toRemove);
                await SaveCurrenciesAsync(currencies);
            }
        }

        public async Task UpdateCurrencyAsync(Currency currency)
        {
            var currencies = await LoadCurrenciesAsync();
            int index = currencies.FindIndex(c => c.CharCode == currency.CharCode);
            if (index >= 0)
            {
                currencies[index] = currency;
                await SaveCurrenciesAsync(currencies);
            }
        }
    }
}