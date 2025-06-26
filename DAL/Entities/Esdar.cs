namespace DAL.Entities
{
    public class Esdar
    {
        public int Id { get; set; }
        public int CustomerConsumptionsId { get; set; }
        public CustomerConsumptions? consumptions { get; set; }
        public decimal ConsumptionAmount { get; set; }
        public DateTime EsdarDate { get; set; }
        public bool IsSuccess { get; set; } 
    }
}
