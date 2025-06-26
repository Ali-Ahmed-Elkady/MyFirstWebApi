using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class AppUser :IdentityUser
    {
        public required string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
