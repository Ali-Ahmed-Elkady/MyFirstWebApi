namespace BLL.Dto.Account
{
   public class UserDto
   {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
