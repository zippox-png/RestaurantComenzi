using System;
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

        public ObservableCollection<CartItem> Items { get; set; }

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
            _discountService = new DiscountService();
            _comandaRepository = new ComandaRepository();
            _currentUserId = currentUserId;

            Items = new ObservableCollection<CartItem>(_cartService.Items);

            PlaseazaComandaCommand = new RelayCommand(PlaseazaComanda, CanPlasaComanda);
            StergeProdusCommand = new RelayCommand(StergeProdus);

            CalculeazaTotaluri();
        }

        private void CalculeazaTotaluri()
        {
            TotalBrut = _cartService.CalculeazaTotalBrut();

            // Calculam numarul de comenzi anterioare ale clientului pentru discount
            int istoricComenzi = _comandaRepository.GetComenziClient(_currentUserId).Count;

            Discount = _discountService.CalculeazaDiscountComanda(TotalBrut, istoricComenzi);

            decimal pretDupaDiscount = TotalBrut - Discount;
            CostTransport = _discountService.CalculeazaTransport(pretDupaDiscount);

            TotalFinal = pretDupaDiscount + CostTransport;
        }

        private bool CanPlasaComanda(object obj) => Items.Any();

        private void StergeProdus(object obj)
        {
            if (obj is CartItem item)
            {
                _cartService.StergeProdus(item.Produs);
                Items.Remove(item);
                CalculeazaTotaluri();
            }
        }

        private void PlaseazaComanda(object obj)
        {
            var comanda = new Comanda
            {
                UtilizatorId = _currentUserId,
                PretTotal = TotalFinal,
                CostTransport = CostTransport,
                OraEstimativaLivrare = DateTime.Now.AddMinutes(45) // Livrare standard in 45 min
            };

            // Inserare comanda in BD
            int comandaId = _comandaRepository.InsertComanda(comanda);

            // Curatam cosul
            _cartService.GolesteCosul();
            Items.Clear();
            CalculeazaTotaluri();

            MesajComanda = $"Comanda plasată cu succes! Codul comenzii a fost generat. Timp estimat: 45 min.";
        }
    }
}