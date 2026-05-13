using System;
using System.Windows.Input;
using RestaurantComenzi.BusinessLogic.Services;
using RestaurantComenzi.UI.Core;

namespace RestaurantComenzi.UI.ViewModels
{
    public class RegisterViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        private string _nume;
        public string Nume { get => _nume; set { _nume = value; OnPropertyChanged(); } }

        private string _prenume;
        public string Prenume { get => _prenume; set { _prenume = value; OnPropertyChanged(); } }

        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string _parola;
        public string Parola { get => _parola; set { _parola = value; OnPropertyChanged(); } }

        private string _telefon;
        public string Telefon { get => _telefon; set { _telefon = value; OnPropertyChanged(); } }

        private string _adresa;
        public string Adresa { get => _adresa; set { _adresa = value; OnPropertyChanged(); } }

        private string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        public Action OnRegisterSuccess { get; set; }
        public Action OnCancel { get; set; }

        public RegisterViewModel()
        {
            _authService = new AuthService();
            RegisterCommand = new RelayCommand(ExecuteRegister, CanExecuteRegister);
            CancelCommand = new RelayCommand(o => OnCancel?.Invoke());
        }

        private bool CanExecuteRegister(object obj)
        {
            return !string.IsNullOrWhiteSpace(Nume) && !string.IsNullOrWhiteSpace(Prenume) &&
                   !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Parola) &&
                   !string.IsNullOrWhiteSpace(Telefon) && !string.IsNullOrWhiteSpace(Adresa);
        }

        private void ExecuteRegister(object obj)
        {
            try
            {
                _authService.Register(Nume, Prenume, Email, Parola, Telefon, Adresa);
                ErrorMessage = "Cont creat cu succes!";
                OnRegisterSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare la creare cont. Posibil ca emailul sa existe deja.";
            }
        }
    }
}