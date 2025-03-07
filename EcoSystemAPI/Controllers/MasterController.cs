using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QFDataAccess.Helpers;
using System.Security.Claims;


namespace QuizFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        public int _userID;
        public string _userRole;
        public int _roleID;
        public int _AccountID;
        public string _userName;

        public string _action;
        public string _description;

        protected CustomResponse customResponse = new CustomResponse();

        // AuditLogRepo _AuditLogRepo;

        public MasterController()
        {
            customResponse.StatusCode = 250;
            customResponse.Message = String.Empty;
            _userID = 0;
        }
        protected void ValidateUser()
        {
            var IsAuthenticated = User.Identity.IsAuthenticated;


            if (IsAuthenticated)
            {

                _userName = Convert.ToString(User.FindFirstValue("UserName"));

                _userRole = User.FindFirstValue(ClaimTypes.Role);

                _userID = Convert.ToInt32(User.FindFirstValue("UserID"));

                _AccountID = Convert.ToInt32(User.FindFirstValue("AccountID"));

                _roleID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));
            }
        }

        protected void CheckPermission(string permissionCode)
        {
            var IsAuthenticated = User.Identity.IsAuthenticated;


            if (IsAuthenticated)
            {

                // _userID = Convert.ToInt32(User.FindFirstValue("UserID"));

                // _userRole = User.FindFirstValue(ClaimTypes.Role);

                _roleID = Convert.ToInt32(User.FindFirstValue("RoleID"));

                //  _Email = User.FindFirstValue(ClaimTypes.Name);


            }
        }



        protected void Logout()
        {
            if (User.Identity.IsAuthenticated)
            {

            }
        }


    }
}
