using BLL.Dto;
using DAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BLL.Services.Users
{
   public class User : IUser
    {
        private readonly UserManager<AppUser> user;
        private readonly SignInManager<AppUser> SignIn;
        private readonly RoleManager<IdentityRole> role;
        public User(UserManager<AppUser> User , SignInManager<AppUser>signIn,RoleManager<IdentityRole>Role)
        {
            user = User;
            SignIn = signIn;
            role = Role;
        }

        public async Task<bool> AddRoles(string RoleName)
        {
            try
            {
                if (RoleName is null)
                    throw new Exception("Name Can not Be null");
                var result = await role.CreateAsync(new IdentityRole(RoleName));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddUsers(string username ,string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Email cannot be empty");
                
            }

            var AppsUser = new AppUser
            {
                Name = username,
                Email = username + "@gmail.com",
                UserName = username
            };

            var result = await user.CreateAsync(AppsUser, password);
            if (result.Succeeded) return true;
            return false;
            
        }

        public async Task<bool> DeleteUser(string Id)
        { 
            var result = await user.FindByIdAsync(Id);
            if (result is null)
            {
                throw new Exception("user not found");
            }
            if (result.IsDeleted)
            {
                throw new Exception("user is Deleted");
            }
            result.IsDeleted = true;
            await user.UpdateAsync(result);
            return true;
        }
        // take the parameters as object not props 
        //split the edit function into two one for the user the other for update password 
        public async Task<bool> EditUser(UserDto user1) 
        {
           
            var User = await user.FindByNameAsync(user1.username);
            if (User is null)
            {
                throw new Exception("User not found");
            }
            if (!User.IsDeleted)
            {
                User.UserName = user1.username;
                User.Email = user1.NewUserName + "@gmail.com";
                if (user1.NewPassword != null && user1.password != null)
                {
                    await user.ChangePasswordAsync(User, user1.password, user1.NewPassword);
                }

                var result = await user.UpdateAsync(User);
                if (!result.Succeeded)
                {
                    throw new Exception("User update failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                return true;
            }throw new Exception("User Is Deleted Cannot Be edited ");
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await user.Users.ToListAsync();
        }

        public async Task<bool> Login(string username, string password)
        {
            var result = await user.FindByNameAsync(username);
            if (result == null)
            {
                throw new Exception("user not found");
            }
            var result2 =await SignIn.PasswordSignInAsync(result, password, false,false);
            if (result2.Succeeded)
            {
              
                return true;
            }
            return false;
           
        }
    }
}
