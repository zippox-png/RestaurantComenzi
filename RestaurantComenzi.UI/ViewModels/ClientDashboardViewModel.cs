using System.Collections.ObjectModel;
using System.Windows.Input;
using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.DataAccess.Repositories;
using RestaurantComenzi.UI.Core;

namespace RestaurantComenzi.UI.ViewModels
{
    public class ClientDashboardViewModel : ObservableObject
    {
        private readonly ComandaRepository _comandaRepository;
        private readonly int _currentUserId;

        private ObservableCollection<Comanda> _comenziClient;
        public ObservableCollection<Comanda> ComenziClient
        {
            get => _comenziClient;
            set { _comenziClient = value; OnPropertyChanged(); }
        }

        public ICommand AnuleazaComandaCommand { get; }

        public ClientDashboardViewModel(int utilizatorId)
        {
            _comandaRepository = new ComandaRepository();
            _currentUserId = utilizatorId;
            ComenziClient = new ObservableCollection<Comanda>();

            AnuleazaComandaCommand = new RelayCommand(AnuleazaComanda, CanAnula);

            IncarcaComenzi();
        }

        private void IncarcaComenzi()
        {
            var comenzi = _comandaRepository.GetComenziClient(_currentUserId);
            ComenziClient.Clear();
            foreach (var c in comenzi)
            {
                ComenziClient.Add(c);
            }
        }

        private bool CanAnula(object obj)
        {
            if (obj is Comanda c)
            {
                return c.Stare != "livrata" && c.Stare != "anulata";
            }
            return false;
        }

        private void AnuleazaComanda(object obj)
        {
            if (obj is Comanda c)
            {
                _comandaRepository.UpdateStare(c.Id, "anulata");
                IncarcaComenzi(); 
            }
        }
    }
}