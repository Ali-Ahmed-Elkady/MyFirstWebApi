namespace DAL.Entities
{
    public class Tariff
    {
        public int Id { get; set; }
        public int CustomerServiceMethod { get; set; }
        public DateTime ActivationDate { get; set; }
        public int ZeroReading { get; set; }
        public int ActivityTypeId { get; set; }
        public ActivityType? activity { get; set; }
        public List<TariffSteps>? TariffSteps { get; set; }
    }
}
