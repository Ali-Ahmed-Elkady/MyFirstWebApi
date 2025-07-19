namespace BLL.Dto
{
    public class EsdarDto
    {
        public int Id { get; set; }
        public int CustomerConsumptionsId { get; set; }
        public decimal ConsumptionKw { get; set; }
        public decimal ConsumptionAmount { get; set; }
        public DateTime EsdarDate { get; set; }
        public bool IsSuccess { get; set; }
    }
}
