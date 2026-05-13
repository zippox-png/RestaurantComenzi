using System;

namespace RestaurantComenzi.DataAccess.Entities
{
    public class Preparat
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
        public decimal Pret { get; set; }
        public string CantitatePortie { get; set; }
        public decimal CantitateTotala { get; set; }
        public int CategorieId { get; set; }
        public string ImaginePath { get; set; }
        public bool Disponibil { get; set; }
    }
}