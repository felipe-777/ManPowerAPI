using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenPowerAPI.Models
{
    public class Timekeeping
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public string Start { get; set; }
        public string Break { get; set; }
        public string Return { get; set; }
        public string End { get; set; }
    }
}
