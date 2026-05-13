using System;
using System.Configuration;

namespace RestaurantComenzi.BusinessLogic.Services
{
    public class DiscountService
    {
        public decimal CalculeazaDiscountComanda(decimal pretTotal, int numarComenziInInterval)
        {
            // Citim valorile din App.config
            decimal sumaMinima = decimal.Parse(ConfigurationManager.AppSettings["MinOrderValueForDiscount"]);
            int comenziNecesare = int.Parse(ConfigurationManager.AppSettings["OrdersCountThreshold"]);
            decimal procentDiscount = decimal.Parse(ConfigurationManager.AppSettings["OrderDiscountPercentage"]);

            // Regula 1: Comanda este mai mare decat suma 'y'
            // Regula 2: Clientul are mai mult de 'z' comenzi
            if (pretTotal >= sumaMinima || numarComenziInInterval >= comenziNecesare)
            {
                decimal reducere = pretTotal * (procentDiscount / 100);
                return reducere;
            }

            return 0; // Nu se aplica discount
        }

        public decimal CalculeazaTransport(decimal pretFinalDupaDiscount)
        {
            decimal pragTransportGratuit = decimal.Parse(ConfigurationManager.AppSettings["FreeTransportThreshold"]);
            decimal costTransport = decimal.Parse(ConfigurationManager.AppSettings["TransportFee"]);

            // Daca comanda este mai mica decat suma 'a', plateste transport 'b'
            if (pretFinalDupaDiscount < pragTransportGratuit)
            {
                return costTransport;
            }

            return 0; // Transport gratuit
        }
    }
}