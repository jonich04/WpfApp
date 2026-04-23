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
            UseJson = true;
            UseSqlite = false;
            
        }

        private void Save()
        {
            if (UseJson)
            {
                InfoText = "Сохранено: использование JSON";
            }
            else if (UseSqlite)
            {
                UseJson = true;
                UseSqlite = false;
            }
        }

        private void Close()
        {
            RequestClose?.Invoke(this, System.EventArgs.Empty);
        }
    }
}