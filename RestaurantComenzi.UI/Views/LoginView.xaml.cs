using RestaurantComenzi.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantComenzi.UI.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            PassBox.PasswordChanged += PassBox_PasswordChanged;
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Parola = PassBox.Password;
            }
        }
    }
}