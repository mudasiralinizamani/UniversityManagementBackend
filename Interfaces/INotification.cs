namespace UniversityManagementBackend.Interfaces;

public interface INotification
{
  Task CreateNotificationAsync(string text, string userId, string type);

  Task<IEnumerable<NotificationModel>> GetNotificationsAsync();

  Task<NotificationModel?> GetNotificationByIdAsync(Guid? id);

  Task<IEnumerable<NotificationModel>> GetUserNotifications(string? user_id);
}