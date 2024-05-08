namespace MenPowerAPI.Models.Interfaces
{
    public interface IAuthenticationService
    {
        public string CreateJWTToken(User user);
    }
}
