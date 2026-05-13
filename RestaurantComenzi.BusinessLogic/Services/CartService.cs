using RestaurantComenzi.BusinessLogic.Models;
using System.Collections.Generic;
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
        public List<CartItem> Items { get; private set; }

        public CartService()
        {
            Items = new List<CartItem>();
        }

        public void AdaugaProdus(MeniuRestaurantModel produs, int cantitate = 1)
        {
            var existingItem = Items.FirstOrDefault(i => i.Produs.Id == produs.Id && i.Produs.TipProdus == produs.TipProdus);
            if (existingItem != null)
            {
                existingItem.Cantitate += cantitate;
            }
            else
            {
                Items.Add(new CartItem { Produs = produs, Cantitate = cantitate });
            }
        }

        public void StergeProdus(MeniuRestaurantModel produs)
        {
            var item = Items.FirstOrDefault(i => i.Produs.Id == produs.Id && i.Produs.TipProdus == produs.TipProdus);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal CalculeazaTotalBrut()
        {
            return Items.Sum(i => i.Produs.Pret * i.Cantitate);
        }

        public void GolesteCosul()
        {
            Items.Clear();
        }
    }
}