using System.Diagnostics;
using esust.Repositories;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MainMenuController : MasterController
    {
        private MenuRepo menuRepo;

        public MainMenuController(MenuRepo _menuRepo)
        {
            menuRepo = _menuRepo;
        }

        [HttpGet("MenuList")]
        public IActionResult MenuList()
        {
           var menus = menuRepo.GetAll();
            customResponse.Data = menus;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }
        

    }
}
