using System;
using System.Configuration;

namespace RestaurantComenzi.BusinessLogic.Services
{
    public class DiscountService
    {
        public decimal CalculeazaDiscountComanda(decimal pretTotal, int numarComenziInInterval)
        {
            decimal sumaMinima = decimal.Parse(ConfigurationManager.AppSettings["MinOrderValueForDiscount"]);
            int comenziNecesare = int.Parse(ConfigurationManager.AppSettings["OrdersCountThreshold"]);
            decimal procentDiscount = decimal.Parse(ConfigurationManager.AppSettings["OrderDiscountPercentage"]);

           
            if (pretTotal >= sumaMinima || numarComenziInInterval >= comenziNecesare)
            {
                decimal reducere = pretTotal * (procentDiscount / 100);
                return reducere;
            }

            return 0;
        }

        public decimal CalculeazaTransport(decimal pretFinalDupaDiscount)
        {
            decimal pragTransportGratuit = decimal.Parse(ConfigurationManager.AppSettings["FreeTransportThreshold"]);
            decimal costTransport = decimal.Parse(ConfigurationManager.AppSettings["TransportFee"]);

            if (pretFinalDupaDiscount < pragTransportGratuit)
            {
                return costTransport;
            }

            return 0; 
        }
    }
}