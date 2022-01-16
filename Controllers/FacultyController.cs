namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class FacultyController : ControllerBase
{
  private readonly IFaculty _facultyService;
  private readonly UserManager<UserModel> _userManager;
  private readonly INotification _notificationService;
  private readonly IDepartment _departmentService;

  public FacultyController(IFaculty facultyService, UserManager<UserModel> userManager, INotification notificationService, IDepartment departmentService)
  {
    _facultyService = facultyService;
    _userManager = userManager;
    _notificationService = notificationService;
    _departmentService = departmentService;
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
  public async Task<ActionResult<FacultyModel>> GetFaculty(string id)
  {
    try
    {
      Guid Id;
      bool converted = Guid.TryParse(id, out Id);

      if (!converted)
        return BadRequest(new { code = "InvalidId", error = "Plz provide a valid Id" });

      FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(Id);

      if (faculty is null)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty does not exist" });

      return Ok(faculty);
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while finding the faculty" });
    }
  }

  [HttpPut]
  [Route("UpdateDean")]
  public async Task<ActionResult<object>> UpdateDean(UpdateFacultyDeanDto dto)
  {
    try
    {
      Guid Id;
      bool converted = Guid.TryParse(dto.FacultyId, out Id);

      if (!converted)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty not found" });

      FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(Id);

      if (faculty is null)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty not found" });

      UserModel newDean = await _userManager.FindByIdAsync(dto.NewDeanId);

      if (newDean is null)
        return BadRequest(new { code = "DeanNotFound", error = "Dean not found" });
      else if (newDean.Role != "Dean")
        return BadRequest(new { code = "UserNotDean", error = "User is not Dean" });

      _facultyService.UpdateFacultyDean(faculty, newDean);
      await _notificationService.CreateNotificationAsync($"Congrats, A new faculty has been assigned to you, {faculty.Name}", newDean.Id, "success");
      await _notificationService.CreateNotificationAsync($"Your faculty has been assigned to {newDean.FullName}", faculty.DeanId, "warning");

      IEnumerable<DepartmentModel> departments = await _departmentService.GetFacultyDepartmentsAsync(faculty.Id.ToString());

      foreach (var department in departments)
      {
        await _notificationService.CreateNotificationAsync($"The faculty has been assigned to another dean, {newDean.FullName}", department.HodId, "info");
      }

      return Ok(new { succeeded = true, faculty = faculty });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while Updating the faculty Dean" });
    }
  }

  [HttpPut]
  [Route("UpdateName")]
  public async Task<ActionResult<object>> UpdateName(UpdateFacultyNameDto dto)
  {
    try
    {
      Guid Id;
      bool converted = Guid.TryParse(dto.FacultyId, out Id);

      if (!converted)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty not found" });

      FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(Id);

      if (faculty is null)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty not found" });

      FacultyModel? Namefaculty = await _facultyService.GetFacultyByNameAsync(dto.NewName);

      if (Namefaculty is not null)
      {
        return BadRequest(new { code = "FacultyNameFound", error = "Faculty with same name already exists" });
      }

      _facultyService.UpdateFacultyName(faculty, dto.NewName);

      IEnumerable<DepartmentModel> departments = await _departmentService.GetFacultyDepartmentsAsync(faculty.Id.ToString());

      foreach (var department in departments)
      {
        _departmentService.UpdateFacultyName(department, dto.NewName);
        await _notificationService.CreateNotificationAsync($"The Faculty Name has been changes to {faculty.Name}", department.HodId, "info");
      }

      return Ok(new { succeeded = true, faculty = faculty });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while Updating the faculty Dean" });
    }
  }

  [HttpDelete]
  [Route("Delete/{faculty_id}")]
  public async Task<ActionResult<object>> DeleteFaculty(string faculty_id)
  {
    try
    {
      Guid Id;
      bool converted = Guid.TryParse(faculty_id, out Id);

      if (!converted)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty is not found" });

      FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(Id);

      if (faculty is null)
        return BadRequest(new { code = "FacultyNotFound", error = "Faculty is not found" });

      _facultyService.DeleteFaculty(faculty);
      await _notificationService.CreateNotificationAsync($"Your faculty has been deleted, {faculty.Name}", faculty.DeanId, "warning");

      IEnumerable<DepartmentModel> departments = await _departmentService.GetFacultyDepartmentsAsync(faculty.Id.ToString());

      foreach (var department in departments)
      {
        _departmentService.DeleteDepartment(department);
        await _notificationService.CreateNotificationAsync($"Your Department has been deleted {department.Name}", department.HodId, "warning");
        await _notificationService.CreateNotificationAsync($"Department has been deleted {department.Name}", department.CourseAdviserId, "warning");
      }

      return Ok(new { succeeded = true });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while finding the faculty" });
    }
  }
}