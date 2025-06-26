namespace BLL.Dto
{
    public class TariffDto
    {
        public int Id { get; set; }
        public int CustomerServiceMethod { get; set; }
        public DateTime ActivationDate { get; set; }
        public int ZeroReading { get; set; }
        public int ActivityTypeId { get; set; }
    }
}
