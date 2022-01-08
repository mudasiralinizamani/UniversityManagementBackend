namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class FacultyController : ControllerBase
{
  private readonly IFaculty _facultyService;
  private readonly UserManager<UserModel> _userManager;
  private readonly INotification _notificationService;

  public FacultyController(IFaculty facultyService, UserManager<UserModel> userManager, INotification notificationService)
  {
    _facultyService = facultyService;
    _userManager = userManager;
    _notificationService = notificationService;
  }

  [HttpPost]
  [Route("Create")]
  public async Task<object> CreateFaculty(CreateFacultyDto dto)
  {
    var user = await _userManager.FindByIdAsync(dto.DeanId);

    if (user is null)
    {
      return BadRequest(new { code = "UserNotFound", error = "User is not found" });
    }
    else if (user.Role != "Dean")
    {
      return BadRequest(new { code = "UserNotDean", error = "User is not type of Dean" });
    }

    FacultyModel? faculty = await _facultyService.GetFacultyByNameAsync(dto.Name);

    if (faculty is not null)
    {
      return BadRequest(new { code = "FacultyNameFound", error = "Faculty with same name already exists" });
    }

    try
    {
      FacultyModel model = new FacultyModel()
      {
        Id = Guid.NewGuid(),
        DeanId = user.Id,
        DeanName = user.FullName,
        Name = dto.Name
      };

      await _facultyService.CreateFacultyAsync(model);
      await _notificationService.CreateNotificationAsync($"Congrats, A new faculty has been assigned to you. {model.Name}", user.Id, "success");
      return Ok(new { succeeded = true });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while creating the faculty" });
    }
  }

  [HttpGet]
  [Route("GetFaculties")]
  public async Task<ActionResult<IEnumerable<FacultyModel>>> GetFaculties()
  {
    return Ok(await _facultyService.GetFacultiesAsync());
  }

  [HttpGet]
  [Route("GetFaculty/{id}")]
  public async Task<ActionResult<FacultyModel>> GetFaculty(Guid id)
  {
    try
    {
      FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(id);

      if (faculty is null)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty does not exist" });

      return Ok(faculty);
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while finding the faculty" });
    }
  }
}