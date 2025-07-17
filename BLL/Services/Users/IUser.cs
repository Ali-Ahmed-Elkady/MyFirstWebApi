using BLL.Dto;
using BLL.Dto.Account;
using BLL.Services.Unified_Response;
using DAL.Entities;

namespace BLL.Services.Users
{
  public  interface IUser
    {
        public Task<UnifiedResponse<List<UserDto>>> GetAll();
        public Task<bool> Register(AccountDto account ,string? userName = null);
        public Task<bool> DeleteUser(string Id , string userName);
        public Task<bool> EditUser(AccountDto account);
        public Task<UnifiedResponse<AuthTokenDto>> Login(AccountDto account);
        public Task<bool> AddRoles(string RoleName);
    }
}
