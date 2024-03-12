using TonaWebApp.Models;

namespace TonaWebApp.Models
{
    public class BoardCreateViewModel
    {
        public User User { get; set; } = null!;

        public Board Board { get; set; } = null!;
    }
}