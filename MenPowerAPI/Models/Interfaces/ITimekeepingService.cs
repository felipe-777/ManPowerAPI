namespace MenPowerAPI.Models.Interfaces
{
    public interface ITimekeepingService
    {
        Task<bool> CreateTimeKeepingRecord(int userId);

        Task<List<Timekeeping>> GetTimeKeepingUser (int userId, DateTime initialDate, DateTime finalDate);
    }
}
