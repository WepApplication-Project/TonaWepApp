namespace TonaWebApp.Models
{
    public class UserAndNotifications
    {
        public User User { get; set; } = null!;

        public List<Notification> Notifications  { get; set; } =  [];
    }
}