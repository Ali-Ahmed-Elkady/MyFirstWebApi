namespace BLL.Dto
{
    public class TariffStepsDto
    {
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public decimal Price { get; set; }
        public decimal ServicePrice { get; set; }
        public bool IsRecalculated { get; set; }
        public int TariffId { get; set; }
    }
}
