namespace RestaurantComenzi.DataAccess.Entities
{
    public class ComandaItem
    {
        public int Id { get; set; }
        public int ComandaId { get; set; }
        public int? PreparatId { get; set; }
        public int? MeniuId { get; set; }
        public int Cantitate { get; set; }
    }
}