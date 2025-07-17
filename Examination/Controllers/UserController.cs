using BLL.Dto.Account;
using BLL.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUser user;
        public UserController(IUser User)
        {
            user = User;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await user.GetAll();
            if (result is null)
            return BadRequest("there is no users in the database");
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser (string id)
        {

            var result = await user.DeleteUser(id ,User.Identity?.Name);
            if (result)
            {
                return Ok("user Deleted Successfully");
            }
            return BadRequest("user Cannot be deleted");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(AccountDto account )
        {
            bool result;
            if (User.Identity?.Name is null)
            {
                 result = await user.Register(account);
            }
            else
            {
                 result = await user.Register(account, User.Identity.Name);
            }
             if (result)
                return Ok("user added successfully");
             return BadRequest("an error happen while adding user");
        }
        [HttpPut]
        public async Task <IActionResult> Edit (AccountDto account)
        {  
           
            var result =  await user.EditUser(account);
            if (result)
            return Ok("user updated successfully");
            return BadRequest("Error");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login (AccountDto account)
        {
          var result = await user.Login(account);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
            
        }

    }
}
