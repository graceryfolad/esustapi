using DataAccess.Helpers;
using DataAccess.IRepositories;
using EcoSystemAPI.Models;
using esust.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoSystemAPI.Repositories
{
    public class UserRepo : BaseRepository<UserModel>
    {
        public UserModel newuser;
        public UserRepo(ESutContextDB dbContext) : base(dbContext)
        {
            newuser = new UserModel();
        }

        public bool CreateAccount(CreateUser user, int usertype)
        {

            var _passwordSalt = PasswordHelper.CreateSalt();
            var _password = PasswordHelper.Create(user.Password, _passwordSalt);

            UserModel account = new UserModel()
            {
                Password = _password,
                Email = user.Email,
                CreatedDate = DateTime.Now,
                updated_at = DateTime.Now,
                isactive = false,
                Salt = _passwordSalt,
                Firstname = user.FirstName,
                Lastname = user.LastName,
               
                PhoneNumber = user.PhoneNumber, 
                UserType  = usertype,
                Status = true,
            }
                ;


            if (this.Insert(account))
            {
                newuser = account;
                return true;
            }
            return false;
        }

        public UserModel getEmailAddress(string username)
        {
            var user = _dbContext.Users.Where(w => w.Email == username).SingleOrDefault();

            if (user == null) return null;
            return user;
        }

        public bool enableuser(int userid, bool isactive)
        {
            var user = _dbContext.Users.Where(w => w.Id == userid).SingleOrDefault();

            user.isactive = isactive;

           if(this.Update(user))
            {
                return true;
            }


            return false;
        }

        public async Task<AccountLoginResponse> Login(Login login)
        {
            var user = await _dbContext.Users.Where(w => w.Email == login.Email).SingleOrDefaultAsync();

            if (user != null)
            {
                if (!user.Status)
                {
                    return new AccountLoginResponse() { AccountID = 0, ErrorMessage = "Account Blocked", Username = login.Email };
                }
                if (PasswordHelper.Validate(login.Password, user.Salt, user.Password))
                {
                    // successful login -- write 
                    user.Password = string.Empty;
                    user.Salt = string.Empty;
                    
                    newuser = user;
                    return new AccountLoginResponse() { AccountID = user.Id, ErrorMessage = "none", Username = login.Email };

                }
                else
                {
                    return new AccountLoginResponse() { AccountID = 0, ErrorMessage = "Invalid credentials", Username = login.Email };
                }


            }



            return new AccountLoginResponse() { AccountID = 0, ErrorMessage = "Invalid credentials", Username = login.Email };
        }

    }
}
