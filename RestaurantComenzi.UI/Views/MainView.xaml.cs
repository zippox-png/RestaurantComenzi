using RestaurantComenzi.UI.ViewModels;
using System.Windows;

namespace RestaurantComenzi.UI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}