using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyAppWPF.Models;

namespace CurrencyAppWPF.Services
{
    public interface IDataService
    {
        Task SaveCurrenciesAsync(List<Currency> currencies);
        Task<List<Currency>> LoadCurrenciesAsync();
        Task AddCurrencyAsync(Currency currency);
        Task DeleteCurrencyAsync(string charCode);
        Task UpdateCurrencyAsync(Currency currency);
    }
}