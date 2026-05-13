using System;
using System.Windows.Input;
using RestaurantComenzi.BusinessLogic.Services;
using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.UI.Core;

namespace RestaurantComenzi.UI.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _parola;
        public string Parola
        {
            get => _parola;
            set { _parola = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public Action<Utilizator> OnLoginSuccess { get; set; }
        public Action OnGoToRegister { get; set; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            GoToRegisterCommand = new RelayCommand(o => OnGoToRegister?.Invoke());
        }

        private bool CanExecuteLogin(object obj)
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Parola);
        }

        private void ExecuteLogin(object obj)
        {
            var user = _authService.Login(Email, Parola);
            if (user != null)
            {
                ErrorMessage = string.Empty;
                OnLoginSuccess?.Invoke(user); // Trimitem user-ul catre MainViewModel
            }
            else
            {
                ErrorMessage = "Email sau parolă incorecte!";
            }
        }
    }
}