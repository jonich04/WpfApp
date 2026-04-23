using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace CurrencyAppWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool _useJson;
        public bool UseJson
        {
            get => _useJson;
            set => SetProperty(ref _useJson, value);
        }

        private bool _useSqlite;
        public bool UseSqlite
        {
            get => _useSqlite;
            set => SetProperty(ref _useSqlite, value);
        }

        private string _infoText;
        public string InfoText
        {
            get => _infoText;
            set => SetProperty(ref _infoText, value);
        }
        public ICommand CloseCommand => new RelayCommand(Close);
        public ICommand SaveCommand => new RelayCommand(Save);

        public event System.EventHandler RequestClose;

        public SettingsViewModel()
        {
            LoadSettings();

        }
        private void LoadSettings()
        {
            string settingsPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "CurrencyAppWPF",
                "settings.txt");

            if (System.IO.File.Exists(settingsPath))
            {
                string saved = System.IO.File.ReadAllText(settingsPath);
                if (saved == "SQLITE")
                {
                    UseJson = false;
                    UseSqlite = true;
                }
                else
                {
                    UseJson = true;
                    UseSqlite = false;
                }
            }
            else
            {
                UseJson = true;
                UseSqlite = false;
            }
        }
      
        private void SaveSettings()
        {
            string settingsPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "CurrencyAppWPF",
                "settings.txt");

            string directory = System.IO.Path.GetDirectoryName(settingsPath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            string setting = UseSqlite ? "SQLITE" : "JSON";
            System.IO.File.WriteAllText(settingsPath, setting);
        }
        private void Save()
        {
            SaveSettings();

            if (UseJson)
            {
                InfoText = "Сохранено в JSON";
            }
            else if (UseSqlite)
            {
                InfoText = "Сохранено в SQLite";
            }
        }
        private void Close()
        {
            RequestClose?.Invoke(this, System.EventArgs.Empty);
        }
    }
}