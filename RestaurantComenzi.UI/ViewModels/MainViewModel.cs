using RestaurantComenzi.BusinessLogic.Services;
using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.UI.Core;
using System.Windows.Input;

namespace RestaurantComenzi.UI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private ObservableObject _currentView;
        private Utilizator _currentUser;

        public ObservableObject CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public Utilizator CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == value)
                    return;

                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsLoggedOut));

            }
        }

        public bool IsLoggedIn => CurrentUser != null;
        public bool IsLoggedOut => CurrentUser == null;

        public ICommand ShowMenuCommand { get; }
        public ICommand ShowLoginCommand { get; }
        public ICommand ShowCartCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowAccountCommand { get; }
        public ICommand ShowEmployeeDashboardCommand { get; }
        private readonly CartService _cartService = new CartService();

        public MainViewModel()
        {
            CurrentView = new MenuViewModel(_cartService);

            ShowAccountCommand = new RelayCommand(o =>
            {
                if (!IsLoggedIn)
                    return;

                if (CurrentUser.Rol == "angajat")
                    CurrentView = new EmployeeDashboardViewModel();
                else
                    CurrentView = new ClientDashboardViewModel(CurrentUser.Id);
            });

            ShowMenuCommand = new RelayCommand(o =>
                CurrentView = new MenuViewModel(_cartService));

            ShowLoginCommand = new RelayCommand(o =>
            {
                var loginVm = new LoginViewModel();

                loginVm.OnLoginSuccess = (user) =>
                {
                    CurrentUser = user;
                    CurrentView = new MenuViewModel(_cartService);
                };

                CurrentView = loginVm;
            });

            ShowCartCommand = new RelayCommand(o =>
            {
                if (IsLoggedIn)
                    CurrentView = new CartViewModel(_cartService, CurrentUser.Id);
                else
                    ShowLoginCommand.Execute(null);
            });

            LogoutCommand = new RelayCommand(o =>
            {
                CurrentUser = null;
                CurrentView = new MenuViewModel(_cartService);
            });

            ShowRegisterCommand = new RelayCommand(o =>
                CurrentView = new RegisterViewModel());
        }
    }
}