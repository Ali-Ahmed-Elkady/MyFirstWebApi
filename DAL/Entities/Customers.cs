using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Customers 
    {
        public Customers()
        {
            CreatedAt = DateTime.Now;
            IsDeleted = false;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime InstallationDate { get; set; }
        public long CustomerCode { get; set;}
        public string? Gender { get; set; }
        public long MeterNo { get; set; }
        public DateTime CreatedAt { get; private set; }      
        public DateTime? ModifiedAt { get;  set; }
        [ForeignKey(nameof(AppUser))]
        public string? CreatedBy { get; set; }
        [ForeignKey(nameof(AppUser))]
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public List<CustomerConsumptions> Consumptions { get; set; }
        public int ActivityId { get; set; }
        public ActivityType Activity { get; set; }
        public void UpdateTimestamp()
        {
            ModifiedAt = DateTime.Now; 
        }

    }
}
