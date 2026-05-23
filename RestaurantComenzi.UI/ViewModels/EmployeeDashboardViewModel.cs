using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.DataAccess.Repositories;
using RestaurantComenzi.UI.Core;

namespace RestaurantComenzi.UI.ViewModels
{
    public class EmployeeDashboardViewModel : ObservableObject
    {
        private readonly ComandaRepository _comandaRepository;
        private readonly ProdusRepository _produsRepository;
        public ObservableCollection<Comanda> ToateComenzile { get; set; }

        private ObservableCollection<Comanda> _comenziActive;
        public ObservableCollection<Comanda> ComenziActive
        {
            get => _comenziActive;
            set { _comenziActive = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Preparat> _preparatePeTerminate;
        public ObservableCollection<Preparat> PreparatePeTerminate
        {
            get => _preparatePeTerminate;
            set { _preparatePeTerminate = value; OnPropertyChanged(); }
        }

        public ICommand MutaStareComandaCommand { get; }

        public EmployeeDashboardViewModel()
        {
            _comandaRepository = new ComandaRepository();
            _produsRepository = new ProdusRepository();

            ComenziActive = new ObservableCollection<Comanda>();
            PreparatePeTerminate = new ObservableCollection<Preparat>();

            MutaStareComandaCommand = new RelayCommand(SchimbaStare);

            IncarcaDate();
        }

        private void IncarcaDate()
        {
            var comenzi = _comandaRepository.GetAllComenzi()?.ToList() ?? new();

           
            ToateComenzile = new ObservableCollection<Comanda>(
                comenzi.OrderByDescending(c => c.OraEstimativaLivrare)
            );
            OnPropertyChanged(nameof(ToateComenzile));

            ToateComenzile.Clear();
            ComenziActive.Clear();

            foreach (var c in comenzi)
            {
                var comanda = new Comanda
                {
                    Id = c.Id,
                    UtilizatorId = c.UtilizatorId,
                    Stare = c.Stare,
                    PretTotal = c.PretTotal,
                    CostTransport = c.CostTransport,
                    OraEstimativaLivrare = c.OraEstimativaLivrare
                };

                ToateComenzile.Add(comanda);

                if (c.Stare != "livrata" && c.Stare != "anulata")
                    ComenziActive.Add(comanda);
            }

            int pragEpuizare = int.Parse(ConfigurationManager.AppSettings["LowStockThreshold"]);

            var preparate = _produsRepository.GetPreparate()
                .Where(p => p.CantitateTotala <= pragEpuizare)
                .ToList();

            PreparatePeTerminate.Clear();

            foreach (var p in preparate)
                PreparatePeTerminate.Add(p);
        }


        private void SchimbaStare(object obj)
        {
            if (obj is Comanda c)
            {
                string stareNoua = c.Stare;

                if (c.Stare == "inregistrata") stareNoua = "se pregateste";
                else if (c.Stare == "se pregateste") stareNoua = "a plecat la client";
                else if (c.Stare == "a plecat la client") stareNoua = "livrata";

                if (stareNoua != c.Stare)
                {
                    _comandaRepository.UpdateStare(c.Id, stareNoua);

                    IncarcaDate();
                }
            }
        }
    }
}