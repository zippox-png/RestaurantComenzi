using RestaurantComenzi.BusinessLogic.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RestaurantComenzi.BusinessLogic.Services
{
    public class CartItem
    {
        public MeniuRestaurantModel Produs { get; set; }
        public int Cantitate { get; set; }
    }
    public class CartService
    {
        public ObservableCollection<CartItem> Items { get; } = new ObservableCollection<CartItem>();

        public void AdaugaProdus(MeniuRestaurantModel produs)
        {
            var existing = Items.FirstOrDefault(x => x.Produs.Id == produs.Id);

            if (existing != null)
                existing.Cantitate++;
            else
                Items.Add(new CartItem
                {
                    Produs = produs,
                    Cantitate = 1
                });
        }

        public void StergeProdus(MeniuRestaurantModel produs)
        {
            var item = Items.FirstOrDefault(x => x.Produs.Id == produs.Id);
            if (item != null)
                Items.Remove(item);
        }

        public void GolesteCosul()
        {
            Items.Clear();
        }

        public decimal CalculeazaTotalBrut()
        {
            return Items.Sum(x => x.Produs.Pret * x.Cantitate);
        }
    }

}