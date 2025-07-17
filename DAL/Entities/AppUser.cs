using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class AppUser : IdentityUser
    {

        public required string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public void CreateUser(string? CreatedBy = null)
        {
            if (string.IsNullOrEmpty(CreatedBy))
            {
                this.CreatedBy = Name;
            }
            else
            {
                this.CreatedBy = CreatedBy;
            }
            CreatedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public void EditUser(string username)
        {
            ModifiedBy = username;
            ModifiedOn = DateTime.Now;
        }
        public void DeleteUser(string username)
        {
            ModifiedBy = username;
            ModifiedOn = DateTime.Now;
            IsDeleted = true;
            IsActive = false;
        }
        public void ToggleUserStatus(string username)
        {
            ModifiedBy = username;
            ModifiedOn = DateTime.Now;
            IsActive = !IsActive;
        }

    }
}
