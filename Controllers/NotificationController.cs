namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
  private readonly INotification _notificationService;

  public NotificationController(INotification notificationService)
  {
    _notificationService = notificationService;
  }

  [HttpGet]
  [Route("GetNotifications")]
  public async Task<ActionResult<IEnumerable<NotificationModel>>> GetNotifications()
  {
    return Ok(await _notificationService.GetNotificationsAsync());
  }

  [HttpGet]
  [Route("GetUserNotifications/{user_id}")]
  public async Task<ActionResult<IEnumerable<NotificationModel>>> GetUserNotifications(string user_id)
  {
    if (user_id == "")
      return BadRequest(new { code = "EmptyId, ", error = "Plz provide User Id" });

    return Ok(await _notificationService.GetUserNotifications(user_id));
  }
}