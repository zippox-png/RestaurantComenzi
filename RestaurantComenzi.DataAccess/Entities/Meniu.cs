namespace RestaurantComenzi.DataAccess.Entities
{
    public class Meniu
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
        public int CategorieId { get; set; }
        public bool Disponibil { get; set; }
    }
}