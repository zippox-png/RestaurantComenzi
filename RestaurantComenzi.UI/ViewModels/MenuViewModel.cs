using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RestaurantComenzi.BusinessLogic.Models;
using RestaurantComenzi.DataAccess.Repositories;
using RestaurantComenzi.UI.Core;

namespace RestaurantComenzi.UI.ViewModels
{
    public class MenuViewModel : ObservableObject
    {
        private readonly ProdusRepository _produsRepository;
        private ObservableCollection<MeniuRestaurantModel> _toateProdusele; // Lista originala

        private ObservableCollection<MeniuRestaurantModel> _produseAfisate;
        public ObservableCollection<MeniuRestaurantModel> ProduseAfisate
        {
            get => _produseAfisate;
            set { _produseAfisate = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ExecutaFiltrare(); // Filtreaza automat cand tastezi
            }
        }

        public ICommand AddToCartCommand { get; }

        public MenuViewModel()
        {
            _produsRepository = new ProdusRepository();
            _toateProdusele = new ObservableCollection<MeniuRestaurantModel>();
            ProduseAfisate = new ObservableCollection<MeniuRestaurantModel>();

            AddToCartCommand = new RelayCommand(AdaugaInCos);
            IncarcaMeniul();
        }

        private void IncarcaMeniul()
        {
            var preparate = _produsRepository.GetPreparate();
            _toateProdusele.Clear();

            foreach (var p in preparate)
            {
                _toateProdusele.Add(new MeniuRestaurantModel
                {
                    Id = p.Id,
                    TipProdus = "Preparat",
                    Denumire = p.Denumire,
                    Pret = p.Pret,
                    CantitatiAfisate = p.CantitatePortie,
                    Disponibil = p.Disponibil,
                    Alergeni = "Alergeni nedefiniti" // Aici vei lega AlergenRepository ulterior
                });
            }

            ExecutaFiltrare();
        }

        private void ExecutaFiltrare()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ProduseAfisate = new ObservableCollection<MeniuRestaurantModel>(_toateProdusele);
            }
            else
            {
                var filtrate = _toateProdusele
                    .Where(p => p.Denumire.ToLower().Contains(SearchText.ToLower()) ||
                                p.Alergeni.ToLower().Contains(SearchText.ToLower()))
                    .ToList();

                ProduseAfisate = new ObservableCollection<MeniuRestaurantModel>(filtrate);
            }
        }

        private void AdaugaInCos(object parametruId)
        {
            // Apel catre CartService folosind ID-ul produsului
        }
    }
}