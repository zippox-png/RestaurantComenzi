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
                _currentUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public ICommand ShowMenuCommand { get; }
        public ICommand ShowLoginCommand { get; }
        public ICommand ShowCartCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            // Initializare vizualizare: deschidem meniul prima data
            CurrentView = new MenuViewModel();

            ShowMenuCommand = new RelayCommand(o => {
                CurrentView = new MenuViewModel();
            });

            ShowLoginCommand = new RelayCommand(o => {
                var loginVm = new LoginViewModel();

                // Callback succes login
                loginVm.OnLoginSuccess = (user) => {
                    CurrentUser = user;
                    CurrentView = new MenuViewModel();
                };

                // Callback buton register din interiorul login-ului
                loginVm.OnGoToRegister = () => {
                    var regVm = new RegisterViewModel();
                    regVm.OnRegisterSuccess = () => ShowLoginCommand.Execute(null);
                    regVm.OnCancel = () => ShowLoginCommand.Execute(null);
                    CurrentView = regVm;
                };

                CurrentView = loginVm;
            });

            ShowCartCommand = new RelayCommand(o => {
                if (IsLoggedIn)
                    CurrentView = new CartViewModel(new BusinessLogic.Services.CartService(), CurrentUser.Id);
                else
                    ShowLoginCommand.Execute(null);
            });

            LogoutCommand = new RelayCommand(o => {
                CurrentUser = null;
                CurrentView = new MenuViewModel();
            });
        }
    }
}