using MongoDB.Driver;
using TonaWebApp.Models;
using TonaWebApp.Config;

namespace TonaWebApp.Repositories
{
    public class NotificationRepository(MongoDBContext context)
    {
        private readonly IMongoCollection<Notification> _notifications = context.Notifications;

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            try
            {
                var notificationList = await _notifications.Find(_ => true).ToListAsync();
                return notificationList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching notifications: {ex}");
                return [];
            }
        }

        public async Task<Notification> GetNotificationByIdAsync(string id)
        {
            var notification = await _notifications.Find(n => n.Id == id).FirstOrDefaultAsync();
            return notification;
        }

        public async Task<Notification> GetNotificationByUserIdAsync(string id)
        {
            var notification = await _notifications.Find(n => n.User.Id == id).FirstOrDefaultAsync();
            return notification;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _notifications.InsertOneAsync(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding notification: {ex}");
            }
        }

        public async Task<bool> UpdateNotificationAsync(string id, Notification updatedNotification)
        {
            var result = await _notifications.ReplaceOneAsync(n => n.Id == id, updatedNotification);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteNotificationAsync(string id)
        {
            var result = await _notifications.DeleteOneAsync(n => n.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(string id)
        {
            var filter = Builders<Notification>.Filter.Eq(n => n.User.Id, id);
            var notifications = await _notifications.Find(filter).ToListAsync();
            return notifications;
        }

        public async Task<List<Notification>> GetUnreadNotificationsByUserIdAsync(string id)
        {
            var filter = Builders<Notification>.Filter.And(
                Builders<Notification>.Filter.Eq(n => n.User.Id, id),
                Builders<Notification>.Filter.Eq(n => n.IsReaded, false)
            );

            var unreadNotifications = await _notifications.Find(filter).ToListAsync();
            return unreadNotifications;
        }

    }
}
