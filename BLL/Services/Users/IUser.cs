using BLL.Dto;
using DAL.Entities;

namespace BLL.Services.Users
{
  public  interface IUser
    {
        public Task<List<AppUser>> GetAll();
        public Task<bool> AddUsers(string username ,string password);
        public Task<bool> DeleteUser(string Id);
        public Task<bool> EditUser(UserDto user);
        public Task<bool> Login(string username, string password);
    }
}
