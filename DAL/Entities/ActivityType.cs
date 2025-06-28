namespace DAL.Entities
{
    public class ActivityType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Code { get; set; }
        public List<Tariff>? Tariffs { get; set; }
        List<Customers>? Customers { get; set; }
    }
}
