using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.DataAccess.Repositories;
using System;
using System.Collections.Generic;


namespace RestaurantComenzi.BusinessLogic.Services
{
    public class OrderService
    {
        private readonly ComandaRepository _comandaRepository;
        private readonly ProdusRepository _produsRepository;

        public OrderService()
        {
            _comandaRepository = new ComandaRepository();
            _produsRepository = new ProdusRepository();
        }

        public void PlaseazaComanda(int utilizatorId, decimal total, decimal transport, List<CartItem> produse)
        {
            var comanda = new Comanda
            {
                UtilizatorId = utilizatorId,
                PretTotal = total,
                CostTransport = transport,
                OraEstimativaLivrare = DateTime.Now.AddMinutes(45),
                Stare = "inregistrata"
            };

            int newId = _comandaRepository.InsertComanda(comanda);

            foreach (var item in produse)
            {
                var detaliu = new ComandaItem
                {
                    ComandaId = newId,
                    Cantitate = item.Cantitate
                };

                if (item.Produs.TipProdus == "Preparat")
                    detaliu.PreparatId = item.Produs.Id;
                else
                    detaliu.MeniuId = item.Produs.Id;

                _comandaRepository.InsertDetaliuComanda(detaliu);
            }
        }

        public List<Comanda> GetIstoricClient(int utilizatorId)
        {
            return _comandaRepository.GetComenziClient(utilizatorId);
        }

        public List<Comanda> GetAllOrders()
        {
            return _comandaRepository.GetAllComenzi();
        }

        public void SchimbaStareComanda(int comandaId, string stareNoua, List<ComandaItem> produseComanda = null)
        {
            _comandaRepository.UpdateStare(comandaId, stareNoua);

            if (stareNoua.ToLower() == "livrata" && produseComanda != null)
            {
                foreach (var item in produseComanda)
                {
                    if (item.PreparatId.HasValue)
                    {
                        _produsRepository.UpdateStoc(item.PreparatId.Value, item.Cantitate);
                    }
                }
            }
        }

        public void AnuleazaComanda(int comandaId)
        {
            _comandaRepository.UpdateStare(comandaId, "anulata");
        }
        
    }
}