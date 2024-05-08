namespace MenPowerAPI.Models.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CheckUserNameExistAsync(string username);
        public Task<bool> CheckEmailExistAsync(string email);
        public Task<string> CreateUserRecordValidate(User user);
        public string CheckPasswordStrength(string password);

    }
}
