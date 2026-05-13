using System.Collections.Generic;

namespace RestaurantComenzi.BusinessLogic.Models
{
    public class MeniuRestaurantModel
    {
        public int Id { get; set; }

        public string TipProdus { get; set; }

        public string Denumire { get; set; }
        public decimal Pret { get; set; }

        public string CantitatiAfisate { get; set; }

        public string Alergeni { get; set; }

        public string ImaginePath { get; set; }
        public bool Disponibil { get; set; }
    }
}