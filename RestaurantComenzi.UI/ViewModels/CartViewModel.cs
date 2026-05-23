using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RestaurantComenzi.BusinessLogic.Services;
using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.DataAccess.Repositories;
using RestaurantComenzi.UI.Core;

namespace RestaurantComenzi.UI.ViewModels
{
    public class CartViewModel : ObservableObject
    {
        private readonly CartService _cartService;
        private readonly DiscountService _discountService;
        private readonly ComandaRepository _comandaRepository;
        private readonly int _currentUserId;

        public ObservableCollection<CartItem> Items => _cartService.Items;

        private decimal _totalBrut;
        public decimal TotalBrut { get => _totalBrut; set { _totalBrut = value; OnPropertyChanged(); } }

        private decimal _discount;
        public decimal Discount { get => _discount; set { _discount = value; OnPropertyChanged(); } }

        private decimal _costTransport;
        public decimal CostTransport { get => _costTransport; set { _costTransport = value; OnPropertyChanged(); } }

        private decimal _totalFinal;
        public decimal TotalFinal { get => _totalFinal; set { _totalFinal = value; OnPropertyChanged(); } }

        private string _mesajComanda;
        public string MesajComanda { get => _mesajComanda; set { _mesajComanda = value; OnPropertyChanged(); } }

        public ICommand PlaseazaComandaCommand { get; }
        public ICommand StergeProdusCommand { get; }

        public CartViewModel(CartService cartService, int currentUserId)
        {
            _cartService = cartService;
            _currentUserId = currentUserId;
            _discountService = new DiscountService();
            _comandaRepository = new ComandaRepository();

            PlaseazaComandaCommand = new RelayCommand(PlaseazaComanda);
            StergeProdusCommand = new RelayCommand(StergeProdus);

            Recalculeaza();
        }

        private void Recalculeaza()
        {
            TotalBrut = _cartService.CalculeazaTotalBrut();

            int istoric = _comandaRepository.GetComenziClient(_currentUserId).Count;

            Discount = _discountService.CalculeazaDiscountComanda(TotalBrut, istoric);

            decimal dupa = TotalBrut - Discount;
            CostTransport = _discountService.CalculeazaTransport(dupa);

            TotalFinal = dupa + CostTransport;
        }

        private void StergeProdus(object obj)
        {
            if (obj is CartItem item)
            {
                _cartService.StergeProdus(item.Produs);
                Recalculeaza();
            }
        }

        private void PlaseazaComanda(object obj)
        {
            var comanda = new Comanda
            {
                UtilizatorId = _currentUserId,
                PretTotal = TotalFinal,
                CostTransport = CostTransport,
                OraEstimativaLivrare = System.DateTime.Now.AddMinutes(45)
            };

            _comandaRepository.InsertComanda(comanda);

            _cartService.GolesteCosul();

            Recalculeaza();

            MesajComanda = "Comanda plasată cu succes!";
        }
    }
}