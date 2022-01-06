namespace UniversityManagementBackend.Models;

public class NotificationModel
{
  public Guid Id { get; set; }
  public string Text { get; set; } = string.Empty;
  public string UserId { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
}