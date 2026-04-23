using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyAppWPF.Models;

namespace CurrencyAppWPF.Services
{
    public class CurrencyApiService
    {
        private readonly HttpClient _httpClient;

        public CurrencyApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Currency>> LoadCurrenciesFromApiAsync()
        {
            try
            {
                string url = "https://www.cbr-xml-daily.ru/daily_json.js";
                string json = await _httpClient.GetStringAsync(url);

                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                JsonElement valute = root.GetProperty("Valute");

                List<Currency> currencies = new List<Currency>();

                foreach (JsonProperty property in valute.EnumerateObject())
                {
                    JsonElement currencyData = property.Value;

                    Currency currency = new Currency
                    {
                        Id = currencyData.GetProperty("ID").GetString(),
                        CharCode = currencyData.GetProperty("CharCode").GetString(),
                        NumCode = currencyData.GetProperty("NumCode").GetString(),
                        Nominal = currencyData.GetProperty("Nominal").GetInt32(),
                        Name = currencyData.GetProperty("Name").GetString(),
                        Value = currencyData.GetProperty("Value").GetDouble(),
                        Previous = currencyData.GetProperty("Previous").GetDouble(),
                        IsUserAdded = false,
                        AddedDate = DateTime.Now
                    };

                    currencies.Add(currency);
                }

                return currencies;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
                return new List<Currency>();
            }
        }
    }
}