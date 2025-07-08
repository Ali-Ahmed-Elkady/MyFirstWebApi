using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class CustomerConsumptions
    {
        public int Id { get; set; }
        public long CustomerCode { get; set; }      
        public Customers? customer { get; set; }
        public decimal ConsumptionKw { get; set; }
        public decimal ConsumptionAmount { get; set; }
        public DateTime Month { get; set; }
    }
}
