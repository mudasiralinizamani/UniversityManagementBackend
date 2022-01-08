namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
  private readonly IDepartment _departmentService;
  private readonly UserManager<UserModel> _userManager;
  private readonly IFaculty _facultyService;
  private readonly INotification _notificationService;

  public DepartmentController(IDepartment departmentService, UserManager<UserModel> userManager, IFaculty facultyService, INotification notificationService)
  {
    _departmentService = departmentService;
    _userManager = userManager;
    _facultyService = facultyService;
    _notificationService = notificationService;
  }

  [HttpPost]
  [Route("Create")]
  public async Task<ActionResult<object>> CreateDepartment(CreateDepartmentDto dto)
  {
    UserModel hod = await _userManager.FindByIdAsync(dto.HodId);

    if (hod is null)
      return BadRequest(new { code = "HodNotFound", error = "Head Of Department is not found" });
    else if (hod.Role != "Hod")
      return BadRequest(new { code = "UserNotHod", error = "Plz provide id of Head Of Department" });

    UserModel courseAdviser = await _userManager.FindByIdAsync(dto.CourseAdviserId);

    if (courseAdviser is null)
      return BadRequest(new { code = "CourseAdviserNotFound", error = "Course Adviser is not found" });
    else if (courseAdviser.Role != "CourseAdviser")
      return BadRequest(new { code = "UserNotCourseAdviser", error = "Plz provide id of Course Adviser" });

    FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(dto.FacultyId);

    if (faculty is null)
      return BadRequest(new { code = "FacultyNotFound", error = "Faculty is not found" });

    DepartmentModel? department = await _departmentService.GetDepartmentByNameAsync(dto.Name);

    if (department is not null)
      return BadRequest(new { code = "DepartmentNameFound", error = "Department with the same name is found" });

    try
    {
      DepartmentModel model = new()
      {
        CourseAdviserId = courseAdviser.Id,
        CourseAdviserName = courseAdviser.FullName,
        FacultyId = faculty.Id.ToString(),
        FacultyName = faculty.Name,
        HodId = hod.Id,
        HodName = hod.FullName,
        Id = Guid.NewGuid(),
        Name = dto.Name
      };

      await _departmentService.CreateDepartmentAsync(model);
      await _notificationService.CreateNotificationAsync($"Congrats, A new Department has been assigned to you. {hod.FullName}", hod.Id, "success");
      await _notificationService.CreateNotificationAsync($"Congrats, A new Department has been assigned to you. {courseAdviser.FullName}", courseAdviser.Id, "success");
      await _notificationService.CreateNotificationAsync($"A new Department has been added to your faculty. {dto.Name}", faculty.DeanId, "info");
      return Ok(new { succeeded = true });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return BadRequest(new { code = "ServerError", error = "Error occurred while creating department" });
    }
  }

  [HttpGet]
  [Route("GetDepartments")]
  public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetDepartments()
  {
    return Ok(await _departmentService.GetDepartmentsAsync());
  }

  [HttpGet]
  [Route("GetDepartment/{department_id}")]
  public async Task<ActionResult<DepartmentModel>> GetDepartment(Guid department_id)
  {
    DepartmentModel? department = await _departmentService.GetDepartmentByIdAsync(department_id);

    if (department is null)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    return Ok(department);
  }
}
