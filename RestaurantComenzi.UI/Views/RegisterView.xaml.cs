using System.Windows;
using System.Windows.Controls;
using RestaurantComenzi.UI.ViewModels;

namespace RestaurantComenzi.UI.Views
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
            RegPassBox.PasswordChanged += RegPassBox_PasswordChanged;
        }

        private void RegPassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.Parola = RegPassBox.Password;
            }
        }
    }
}