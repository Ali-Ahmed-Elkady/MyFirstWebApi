namespace DAL.Entities
{
    public class TariffSteps
    {
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public decimal Price { get; set; }
        public decimal ServicePrice { get; set; }
        public bool IsRecalculated { get; set; }
        public int RecalculationEdge { get; set; }
        public decimal RecalculationAddedAmount { get; set; }
        public int TariffId { get; set; }
        public Tariff? Tariff { get; set; }
    }
}
