using DAL.Entities;

namespace BLL.Dto
{
    public class CustomerConsumptionDTO
    {
        public long CustomerCode { get; set; }
        public decimal ConsumptionKw { get; set; }
        public DateTime Month { get; set; }
    }
}
