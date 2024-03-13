using TonaWebApp.Models;

namespace TonaWebApp.Models
{
    public class HomeIndexViewModel
    {
        public User User { get; set; } = null!;

        public List<Board> Boards { get; set; } = [];

        public string SelectedTag { get; set; } = null!;

        public List<string> TagsList { get; set; } = [];
    }
}