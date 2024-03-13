using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Repositories;

namespace TonaWebApp.ViewComponents
{
    public class NavbarViewComponent(NotificationRepository notificationRepository) : ViewComponent
    {
        private readonly NotificationRepository _notificationRepository = notificationRepository;

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var notifications = await _notificationRepository.GetUnreadNotificationsByUserIdAsync(userId);
            return View(notifications);
        }
    }
}
