using CardData.Helpers;
using DataAccess.Helpers;
using EcoSystemAPI.Helpers;
using EcoSystemAPI.Models;
using EcoSystemAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using QFDataAccess.Helpers;
using QuizFramework.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcoSystemAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : MasterController
    {
        UserRepo userRepo;
       
        public AccountController(UserRepo _userRepo)
        {
            userRepo = _userRepo;
           
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                customResponse.Message = "Validation Error";
                customResponse.Error = ModelState;
                return BadRequest(customResponse);
            }

            AccountLoginResponse loginresp = await userRepo.Login(login);

            if (loginresp.AccountID == 0)
            {
                // customResponse.Error = loginresp;
                return Unauthorized(loginresp);
            }

            var token = this.createtoken(userRepo.newuser, loginresp);

            return Ok(token);
        }
        //[HttpPost("Register")]
        //public IActionResult Register(CreateUser create)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        customResponse.Message = "Validation Error";
        //        customResponse.Error = ModelState;
        //        return BadRequest(customResponse);
        //    }

        //    var checkemail = userRepo.getEmailAddress(create.Email);
        //    if (checkemail != null)
        //    {
        //        customResponse.Message = "Email Address already exist";

        //        return BadRequest(customResponse);
        //    }

        //    if (userRepo.CreateAccount(create, 920))
        //    {
        //        // create account number for aggregator
        //        string card = GenerateCardHelper.GenerateVoucherCode(10);

        //        AggAccount account = new AggAccount()
        //        {
        //            AccountNumber = card,
        //            UserId = userRepo.newuser.Id,
        //            Balance = 0,
        //            LastUpdated = DateTime.Now,
        //        }
        //            ;
        //        accountRepo.Insert(account);

        //        customResponse.Message = "Registration Successful";
        //        customResponse.StatusCode = 200;
        //        return Ok(customResponse);
        //    }

        //    customResponse.Message = "Registration failed";
        //    return Ok(customResponse);
        //}

        //[HttpPost("VerifyBVN")]
        //public async Task<IActionResult> VerifyBVNAsync(VerifyBVN verify)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        customResponse.Message = "Validation Error";
        //        customResponse.Error = ModelState;
        //        return BadRequest(customResponse);
        //    }

        //    BVNLib bVN = new BVNLib();

        //    string token = await bVN.GetTokenAsync();

        //    bVN.token = token;
        //    Bvn resp = await bVN.VerifyBVNAsync(verify.BVN, verify.FirstName, verify.LastName);

        //    if (resp != null)
        //    {
        //        if (resp.firstname == verify.FirstName.ToUpper() && resp.lastname == verify.LastName.ToUpper())
        //        {
        //            verify.LastName = resp.lastname;
        //            verify.FirstName = resp.firstname;
        //            customResponse.Message = "Perfect matching";
        //            customResponse.Data = verify;
        //            customResponse.StatusCode = 200;
        //        }
        //    }
        //    else
        //    {
        //        customResponse.Message = "BVN invalid";
        //        customResponse.Data = resp;
        //    }

        //    return Ok(customResponse);
        //}

        private LoginResponse createtoken(UserModel appuser, AccountLoginResponse accountLogin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string jwtkey = ConfigHelper.getAppSetting();
            var key = Encoding.ASCII.GetBytes(jwtkey);
            var date = DateTime.Now;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {                            new Claim(ClaimTypes.Role ,appuser.UserType.ToString()),
                                             new Claim("UserName" , accountLogin.Username),
                                             new Claim("AccountID" , accountLogin.AccountID.ToString()),
                                             new Claim("UserID" , appuser.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(3),
                NotBefore = date,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);


            string Tokenstring = tokenHandler.WriteToken(token);

            LoginResponse response = new LoginResponse();
            response.Details = appuser;
            response.Token = Tokenstring;
            response.TokenExpiry = (DateTime)tokenDescriptor.Expires;
            response.UserType = appuser.UserType.ToString();
            response.StatusCode = 200;


            return response;
        }
    }
}
