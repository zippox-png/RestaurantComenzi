using System;

namespace RestaurantComenzi.DataAccess.Entities
{
    public class Utilizator
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Email { get; set; }
        public string Parola { get; set; }
        public string Telefon { get; set; }
        public string AdresaLivrare { get; set; }
        public string Rol { get; set; }
    }
}