using BLL.Dto;
using BLL.Dto.Account;
using BLL.Services.Unified_Response;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace BLL.Services.Users
{
    public class User : IUser
    {
        private readonly UserManager<AppUser> user;
        private readonly SignInManager<AppUser> SignIn;
        private readonly RoleManager<IdentityRole> role;
        private readonly IConfiguration configuration;
        public User(UserManager<AppUser> User, SignInManager<AppUser> signIn, RoleManager<IdentityRole> Role, IConfiguration configuration)
        {
            user = User;
            SignIn = signIn;
            role = Role;
            this.configuration = configuration;
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

        public async Task<bool> Register(AccountDto account ,string? userName=null)
        {
            if (account.UserName is null || account.password is null)
            {
                throw new Exception("User name or password cannot be empty");
            }

            var AppsUser = new AppUser
            {
                Name = account.UserName,
                Email = account.UserName + "@gmail.com",
                UserName = account.UserName
            };
            if (userName is null)
            {
                AppsUser.CreateUser();
            }
            else
            {
                AppsUser.CreateUser(userName);
            }
            var result = await user.CreateAsync(AppsUser, account.password);
            if (result.Succeeded) return true;
            return false;
        }

        public async Task<bool> DeleteUser(string Id , string userName)
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
            result.DeleteUser(userName);
            await user.UpdateAsync(result);
            return true;
        }
        // take the parameters as object not props 
        //split the edit function into two one for the user the other for update password 
        public async Task<bool> EditUser(AccountDto account)
        {
            if (account.UserName is null || account.password is null)
            {
                throw new Exception("User name or password cannot be empty");
            }
            var User = await user.FindByNameAsync(account.UserName);
            if (User is null)
            {
                throw new Exception("User not found");
            }
            if (!User.IsDeleted)
            {
                User.UserName = account.UserName;
                User.Email = account.UserName + "@gmail.com";
                var result = await user.UpdateAsync(User);
                if (!result.Succeeded)
                {
                  throw new Exception("User update failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                return true;
            }
            throw new Exception("User Is Deleted Cannot Be edited ");
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await user.Users.ToListAsync();
        }

        public async Task<UnifiedResponse<AuthTokenDto>> Login(AccountDto account)
        {
            try
            {
                if (account.UserName is null || account.password is null)
                {
                    throw new Exception("User name or password cannot be empty");
                }
                var result = await user.FindByNameAsync(account.UserName);
                if (result == null)
                {
                    throw new Exception("user not found");
                }
                if (await user.CheckPasswordAsync(result, account.password))
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, result.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    var roles = await user.GetRolesAsync(result);
                    foreach (var item in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
                    }
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    var SC = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: configuration["JWT:Issuer"],
                        audience: configuration["JWT:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials :SC
                        
                    );
                    AuthTokenDto _token = new AuthTokenDto
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token) ,
                        Expiry = token.ValidTo 
                    };
                    return UnifiedResponse<AuthTokenDto>.SuccessResult(_token);
                }
                return UnifiedResponse<AuthTokenDto>.ErrorResult("Error");
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: " + ex.Message);

            }
        }
    }
}
