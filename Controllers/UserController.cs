namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly UserManager<UserModel> _userManager;

  public UserController(UserManager<UserModel> userManager)
  {
    _userManager = userManager;
  }

  [HttpGet]
  [Route("GetUsers")]
  public async Task<ActionResult> GetUsers()
  {
    return Ok(await _userManager.Users.ToListAsync());
  }

  [HttpGet]
  [Route("GetUser/{id}")]
  public async Task<ActionResult> GetUser(string id)
  {
    var user = await _userManager.FindByIdAsync(id);

    if (user is null)
    {
      return BadRequest(new { code = "UserNotFound", error = "User is not found" });
    }

    return Ok(user);
  }

}