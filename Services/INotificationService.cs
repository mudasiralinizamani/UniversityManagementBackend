namespace UniversityManagementBackend.Services;

public class INotificationService : INotification
{
  private readonly ApiContext _context;

  public INotificationService(ApiContext context)
  {
    _context = context;
  }
  public async Task CreateNotificationAsync(string text, string userId, string type)
  {
    NotificationModel model = new NotificationModel()
    {
      Id = Guid.NewGuid(),
      Text = text,
      Type = type,
      UserId = userId
    };

    await _context.AddAsync(model);
    await _context.SaveChangesAsync();
  }

  public async Task<NotificationModel?> GetNotificationByIdAsync(Guid? id)
  {
    if (id is null)
      throw new ArgumentNullException();

    return (await _context.notifications.FindAsync(id));
  }

  public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync()
  {
    return (await _context.notifications.ToListAsync());
  }

  public async Task<IEnumerable<NotificationModel>> GetUserNotifications(string? user_id)
  {
    if (user_id is null)
      throw new ArgumentNullException();
    return (await _context.notifications.Where(x => x.UserId == user_id).ToListAsync());
  }
}