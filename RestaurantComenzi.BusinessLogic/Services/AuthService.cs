using RestaurantComenzi.DataAccess.Entities;
using RestaurantComenzi.DataAccess.Repositories;
using System;

namespace RestaurantComenzi.BusinessLogic.Services
{
    public class AuthService
    {
        private readonly UtilizatorRepository _utilizatorRepository;

        public AuthService()
        {
            _utilizatorRepository = new UtilizatorRepository();
        }

        public Utilizator Login(string email, string parola)
        {
            return _utilizatorRepository.Login(email, parola);
        }

        public void Register(string nume, string prenume, string email, string parola, string telefon, string adresa)
        {
            var user = new Utilizator
            {
                Nume = nume,
                Prenume = prenume,
                Email = email,
                Parola = parola,
                Telefon = telefon,
                AdresaLivrare = adresa
            };

            _utilizatorRepository.Register(user);
        }
    }
}