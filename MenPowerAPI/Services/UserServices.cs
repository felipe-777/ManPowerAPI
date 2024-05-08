using MenPowerAPI.Context;
using MenPowerAPI.Models;
using MenPowerAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace MenPowerAPI.Services
{
    public class UserServices : IUserService
    {
        private readonly AppDbContext _authContext;
        public UserServices(AppDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task<bool> CheckUserNameExistAsync(string username)
        {
            return _authContext.Users.AnyAsync(x => x.UserName == username);
        }

        public Task<bool> CheckEmailExistAsync(string email)
        {
            return _authContext.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<string> CreateUserRecordValidate(User user)
        {
            if (await CheckUserNameExistAsync(user.UserName))
                return "Username Already Exists";

            if(await CheckEmailExistAsync(user.Email))
                return "Email Already Exists";

            return "";
        }

        public string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();

            if (password.Length < 8)
                sb.AppendLine("Minimum password length should be 8");

            if (!Regex.IsMatch(password, "[a-z]") ||
                !Regex.IsMatch(password, "[A-Z]") ||
                !Regex.IsMatch(password, "[0-9]"))
                sb.AppendLine("Password should be Alphanumeric");

            if (!Regex.IsMatch(password, "[<>,@!#$%^&*()_+\\[\\]{}?:;|'\\,. /~`=\\-]"))
                sb.AppendLine("Password should contain special chars");

            return sb.ToString();
        }

    }
}
