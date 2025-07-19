using DAL.Entities;

namespace BLL.Dto
{
    public class CustomerConsumptionDTO
    {
        public int Id { get; set; }
        public long CustomerCode { get; set; }
        public decimal ConsumptionKw { get; set; }
        public DateTime Month { get; set; }
    }
}
