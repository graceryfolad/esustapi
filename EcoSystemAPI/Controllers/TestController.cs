
using CardData.Helpers;
using DataAccess.Helpers;
using EcoSystemAPI.Helpers;
using EcoSystemAPI.Models;
using EcoSystemAPI.Repositories;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using QFDataAccess.Helpers;

using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        UserRepo userRepo;
       MenuRepo menuRepo;
        PageRepo pageRepo;

        
        public TestController( UserRepo _account, MenuRepo menu, PageRepo page)
        {
            userRepo = _account;
            menuRepo = menu;
            pageRepo = page;
           
        }

        [HttpGet("CreateDB")]
        public async Task<IActionResult> CreatDBAsync()
        {
           
            using (var context = new ESutContextDB())
            {
                try
                {
                   var resp = await context.Database.EnsureCreatedAsync();

                    return Ok(resp);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ErrorLogger.Log(ex.InnerException.Message);
                       
                        return BadRequest(ex.InnerException.Message);
                       
                    }
                    else
                    {
                        ErrorLogger.Log(ex.Message);

                        return BadRequest(ex.Message);
                       
                    }


                    return Ok(false);

                }
               
                
            }
            return Ok();
        }

        [HttpGet("DeleteDB")]
        public async Task<IActionResult> DeleteDBAsync()
        {

            using (var context = new ESutContextDB())
            {
                await context.Database.EnsureDeletedAsync();

            }
            return Ok();
        }      
       
       

        [HttpPost("AdminUser")]
        public async Task<IActionResult> AdminUser(CreateUser user)
        {

            if (userRepo.CreateAccount(user,925))
            {
                string verifycode = PasswordHelper.RandomNumbers(6);
                // create account fr user
                return Ok(userRepo.newuser);
            }

            return Ok();
        }

        [HttpPost("AddMenu")]
        public async Task<IActionResult> AddMenu(CreateMenu createMenu)
        {
           var resp = menuRepo.Insert(new MenuTbl()
           {
               level = createMenu.level,
               Label = createMenu.Label,
               CreatedDate = DateTime.Now,
               IsVisible = true,
               MenuType = createMenu.MenuType,
               MenuOrder = 0
           });
            MenuTbl menuTbl = menuRepo.entry;
            if (resp)
            {
                pageRepo.Insert(new PageTbl()
                {
                    CreatedDate = DateTime.Now,
                    Title = menuTbl.Label,
                    Description = menuTbl.Label,
                    ModifiedDate = DateTime.Now,
                    MenuID = menuTbl.Id
                });
            }

            return Ok(resp);
        }

        //[HttpPost("AddColumn")]
        //public async Task<IActionResult> AddColumn(AddColumn add)
        //{

        //    /*
        //     ALTER TABLE Customers
        //ADD Email varchar(255);
        //     */
        //    int resp = 0;
        //    string alt = string.Empty;

        //    /*
        //     Column type 
        //    1 =int
        //    2 =varchar
        //    3 = double
        //    4 = datetime
        //     */
        //    switch (add.ColumnType)
        //    {
        //        case 1:
        //            alt = $" ALTER TABLE {add.TableName} ADD {add.ColumnName} int";
        //            break;
        //        case 2:
        //            alt = $" ALTER TABLE {add.TableName} ADD {add.ColumnName} varchar({add.ColumnSize})";
        //            break;
        //        case 3:
        //            alt = $" ALTER TABLE {add.TableName} ADD {add.ColumnName} float";
        //            break;
        //        case 4:
        //            alt = $" ALTER TABLE {add.TableName} ADD {add.ColumnName} datetime";
        //            break;
        //        case 5:
        //            alt = $" ALTER TABLE {add.TableName} ADD {add.ColumnName} bit";
        //            break;


        //    }


        //    using (var context = new QFContext())
        //    {
        //        resp = await context.Database.ExecuteSqlRawAsync(alt);

        //    }
        //    return Ok(resp);
        //}

    }
}
