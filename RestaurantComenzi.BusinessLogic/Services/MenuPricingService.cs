using System;
using System.Configuration;

namespace RestaurantComenzi.BusinessLogic.Services
{
    public class MenuPricingService
    {
        public decimal CalculeazaPretMeniu(decimal sumaPreturiPreparate)
        {
            // Citim reducerea 'x' din App.config
            decimal reducereProcent = decimal.Parse(ConfigurationManager.AppSettings["MenuDiscount"]);

            decimal reducereValoare = sumaPreturiPreparate * (reducereProcent / 100);
            return sumaPreturiPreparate - reducereValoare;
        }
    }
}