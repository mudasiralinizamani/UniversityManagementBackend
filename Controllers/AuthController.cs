namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  private readonly UserManager<UserModel> _userManager;
  private RoleManager<IdentityRole> _roleManager;

  public AuthController(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
  {
    _userManager = userManager;
    _roleManager = roleManager;
  }

  [HttpPost]
  [Route("Signup")]
  public async Task<ActionResult> Signup(SignupDto dto)
  {
    if (dto.Role != "ADMIN" && dto.Role != "STUDENT" && dto.Role != "HOD" && dto.Role != "DEAN" && dto.Role != "COURSEADVISER" && dto.Role != "TEACHER")
    {
      return BadRequest(new { code = "InvalidRole", error = "Role does not exists" });
    }

    UserModel user = new UserModel()
    {
      Email = dto.Email,
      FullName = dto.FullName,
      ProfilePic = dto.ProfilePic,
      Role = dto.Role,
      UserName = dto.Email,
    };
    try
    {
      var result = await _userManager.CreateAsync(user, dto.Password);

      IdentityRole newRole = new IdentityRole()
      {
        Name = dto.Role,
      };
      if (result.Succeeded)
      {
        await _roleManager.CreateAsync(newRole);
        await _userManager.AddToRoleAsync(user, dto.Role);
        return Ok(new { succeeded = true });
      }
      return BadRequest(new { code = "UserError", error = result.Errors });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while creating the user" });
    }
  }

  [HttpPost]
  [Route("Signin")]
  public async Task<ActionResult> Signin(SigninDto dto)
  {
    var user = await _userManager.FindByEmailAsync(dto.Email);

    if (user is null)
    {
      return BadRequest(new { code = "UserNotFound", error = "User does not exists" });
    }

    var password = await _userManager.CheckPasswordAsync(user, dto.Password);

    if (password!)
    {
      return BadRequest(new { code = "IncorrectPasssword", error = "Password is incorrect" });
    }

    return Ok(user);
  }
}