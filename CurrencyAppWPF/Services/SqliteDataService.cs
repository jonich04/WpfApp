using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using CurrencyAppWPF.Models;

namespace CurrencyAppWPF.Services
{
    public class SqliteDataService : IDataService
    {
        private readonly string _connectionString;
        public SqliteDataService()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string dbPath = System.IO.Path.Combine(desktopPath, "CurrencyAppWPF", "currencies.db");
            _connectionString = $"Data Source={dbPath}";
            string directory = System.IO.Path.GetDirectoryName(dbPath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
            CreateTable();
        }

        private void CreateTable()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Currencies (
                    Id TEXT PRIMARY KEY,
                    CharCode TEXT NOT NULL UNIQUE,
                    NumCode TEXT,
                    Nominal INTEGER,
                    Name TEXT,
                    Value REAL,
                    Previous REAL,
                    IsUserAdded INTEGER,
                    AddedDate TEXT
                )";

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        public async Task SaveCurrenciesAsync(List<Currency> currencies)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            string deleteSql = "DELETE FROM Currencies";
            using var deleteCmd = new SqliteCommand(deleteSql, connection);
            await deleteCmd.ExecuteNonQueryAsync();
            foreach (var currency in currencies)
            {
                string insertSql = @"
                    INSERT OR REPLACE INTO Currencies 
                    (Id, CharCode, NumCode, Nominal, Name, Value, Previous, IsUserAdded, AddedDate)
                    VALUES (@Id, @CharCode, @NumCode, @Nominal, @Name, @Value, @Previous, @IsUserAdded, @AddedDate)";

                using var insertCmd = new SqliteCommand(insertSql, connection);
                insertCmd.Parameters.AddWithValue("@Id", currency.Id);
                insertCmd.Parameters.AddWithValue("@CharCode", currency.CharCode);
                insertCmd.Parameters.AddWithValue("@NumCode", currency.NumCode);
                insertCmd.Parameters.AddWithValue("@Nominal", currency.Nominal);
                insertCmd.Parameters.AddWithValue("@Name", currency.Name);
                insertCmd.Parameters.AddWithValue("@Value", currency.Value);
                insertCmd.Parameters.AddWithValue("@Previous", currency.Previous);
                insertCmd.Parameters.AddWithValue("@IsUserAdded", currency.IsUserAdded ? 1 : 0);
                insertCmd.Parameters.AddWithValue("@AddedDate", currency.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                await insertCmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Currency>> LoadCurrenciesAsync()
        {
            var currencies = new List<Currency>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT * FROM Currencies";
            using var command = new SqliteCommand(sql, connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var currency = new Currency
                {
                    Id = reader.GetString(0),
                    CharCode = reader.GetString(1),
                    NumCode = reader.GetString(2),
                    Nominal = reader.GetInt32(3),
                    Name = reader.GetString(4),
                    Value = reader.GetDouble(5),
                    Previous = reader.GetDouble(6),
                    IsUserAdded = reader.GetInt32(7) == 1,
                    AddedDate = DateTime.Parse(reader.GetString(8))
                };
                currencies.Add(currency);
            }

            return currencies;
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