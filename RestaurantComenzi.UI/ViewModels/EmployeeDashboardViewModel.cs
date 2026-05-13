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
            // 1. Incarcam comenzile active (practic toate din BD pentru demo, sau apelezi GetToateComenzile daca ai adaugat procedura)
            // Pentru a fi simplu, presupunem ca ai o lista. Aici filtrăm doar ce nu e anulat sau livrat.
            // (Nota: Pentru afisarea la angajati e bine sa adaugi in ComandaRepository o metoda GetAllComenzi() asemanatoare cu cea de client)

            // 2. Incarcam stocurile critice (Citim din App.config)
            int pragEpuizare = int.Parse(ConfigurationManager.AppSettings["LowStockThreshold"]);
            var preparate = _produsRepository.GetPreparate()
                                             .Where(p => p.CantitateTotala <= pragEpuizare)
                                             .ToList();

            PreparatePeTerminate.Clear();
            foreach (var p in preparate)
            {
                PreparatePeTerminate.Add(p);
            }
        }

        private void SchimbaStare(object obj)
        {
            if (obj is Comanda c)
            {
                string stareNoua = c.Stare;

                // Masina de stari simpla pentru fluxul comenzii
                if (c.Stare == "inregistrata") stareNoua = "se pregateste";
                else if (c.Stare == "se pregateste") stareNoua = "a plecat la client";
                else if (c.Stare == "a plecat la client")
                {
                    stareNoua = "livrata";
                    // Cand e livrata, in mod normal aici s-ar actualiza si stocul automat
                }

                if (stareNoua != c.Stare)
                {
                    _comandaRepository.UpdateStare(c.Id, stareNoua);
                    c.Stare = stareNoua;
                    OnPropertyChanged(nameof(ComenziActive)); // Refresh vizual
                }
            }
        }
    }
}