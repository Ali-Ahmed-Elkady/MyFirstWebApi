﻿using BLL.Dto;
using BLL.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Examination.Controllers
{
    [ApiController]
    [Route("[action]")]
    
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
            var result = await user.DeleteUser(id);
            if (result)
            {
                return Ok("user Deleted Successfully");
            }
            return BadRequest("user Cannot be deleted");
        }
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
             var result = await user.AddUsers(username, password);
             if (result)
             return Ok("user added successfully");
             return BadRequest("an error happen while adding user");
        }
        [HttpPut]
        public async Task <IActionResult> Edit (UserDto user1)
        {  
           
            var result =  await user.EditUser(user1);
            if (result)
            return Ok("user updated successfully");
            return BadRequest("Error");
        }
        [HttpPost]
        public async Task<IActionResult> Login (string username ,string password)
        {
          var result = await user.Login(username, password);
            if (result) return Ok("login successfully");
            return BadRequest("password not correct");
        }

    }
}
