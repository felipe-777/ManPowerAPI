using MenPowerAPI.Context;
using MenPowerAPI.Models;
using MenPowerAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace MenPowerAPI.Services
{
    public class TimekeepingService : ITimekeepingService
    {
        private readonly AppDbContext _authContext;
        public TimekeepingService(AppDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<List<Timekeeping>> GetTimeKeepingUser(int userId, DateTime initialDate, DateTime finalDate)
        {
            var time = await _authContext.Timekeeping.Where(x => x.UserId == userId && x.Date >= initialDate && x.Date <= finalDate).ToListAsync();

            return time;
        }

        public Task<bool> CreateTimeKeepingRecord(int userId)
        {
            var timekeeping = _authContext.Timekeeping.Where(x => x.UserId == userId && x.Date == DateTime.Now.Date).FirstOrDefault();

            if(timekeeping == null)
            {
                _authContext.Timekeeping.AddAsync(new Timekeeping
                {
                    UserId = userId,
                    Date = DateTime.Now.Date,
                    Start = DateTime.Now.ToString("HH:mm")
                });
            }
            else if (timekeeping.Break == null)
            {
                timekeeping.Break = DateTime.Now.ToString("HH:mm");
            }
            else if (timekeeping.Return == null)
            {
                timekeeping.Return = DateTime.Now.ToString("HH:mm");
            }
            else if(timekeeping.End == null)
            {
                timekeeping.End = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                return Task.FromResult(false);
            }

            _authContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
