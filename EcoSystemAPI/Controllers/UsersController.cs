using EcoSystemAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;
using VHMSData.Helpers;

namespace EcoSystemAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class UsersController : MasterController
    {
        UserRepo users;
        public UsersController(UserRepo userRepo)
        {
            users = userRepo;
        }

        [HttpGet("UserList")]
        public async Task<IActionResult> UserList()
        {
            ValidateUser();
            if (_roleID == (int)UserType.Aggregator)
            {
                customResponse.Message = "Permission Denied!";
                return Ok(customResponse);
            }

            var list = users.GetAll();
            customResponse.Data = list;
            customResponse.StatusCode = 200;

            return Ok(customResponse);
        }
        [HttpGet("User/{userid}")]
        public async Task<IActionResult> UserDetails(int userid)
        {
            ValidateUser();
            if (_roleID == (int)UserType.Aggregator)
            {
                customResponse.Message = "Permission Denied!";
                return Ok(customResponse);
            }

            var list = users.GetById(userid);
            customResponse.Data = list;
            customResponse.StatusCode = 200;

            return Ok(customResponse);
        }
        [HttpPut("User/{userid}/enable")]
        public async Task<IActionResult> UserEnable(int userid)
        {
            ValidateUser();
            if (_roleID == (int)UserType.Aggregator)
            {
                customResponse.Message = "Permission Denied!";
                return Ok(customResponse);
            }

        customResponse.Data =    users.enableuser(userid,true);

            return Ok(customResponse);
        }

        [HttpPut("User/{userid}/disable")]
        public async Task<IActionResult> UserDisable(int userid)
        {
            ValidateUser();
            if (_roleID == (int)UserType.Aggregator)
            {
                customResponse.Message = "Permission Denied!";
                return Ok(customResponse);
            }

            customResponse.Data =  users.enableuser(userid, false);
            return Ok(customResponse);
        }
    }
}
