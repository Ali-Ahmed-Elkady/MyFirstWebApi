using System.ComponentModel.DataAnnotations;

namespace BLL.Dto
{
   public class UserDto
   {
        public string username { get; set; } = "";
        public string password { get; set; } = "Giza@123456";
        public string? NewPassword { get; set; } 
        public string? NewUserName { get; set; }
        
    }
}
