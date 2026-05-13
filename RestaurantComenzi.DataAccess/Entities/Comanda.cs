using System;

namespace RestaurantComenzi.DataAccess.Entities
{
    public class Comanda
    {
        public int Id { get; set; }
        public Guid CodComanda { get; set; }
        public int UtilizatorId { get; set; }
        public DateTime DataComanda { get; set; }
        public string Stare { get; set; }
        public decimal PretTotal { get; set; }
        public decimal CostTransport { get; set; }
        public DateTime? OraEstimativaLivrare { get; set; }
    }
}