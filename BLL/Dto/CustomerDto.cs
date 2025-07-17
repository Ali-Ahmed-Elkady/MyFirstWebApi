namespace BLL.Dto
{
    public class CustomerDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime InstallationDate { get; set; }
        public long CustomerCode { get; set; }
        public string? Gender { get; set; }
        public long MeterNo { get; set; }
        public int ActivityId { get; set; }
    }
}
