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
    catch (Exception)
    {
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
  public async Task<ActionResult<DepartmentModel>> GetDepartment(string department_id)
  {
    try
    {
      Guid Id;
      bool converted = Guid.TryParse(department_id, out Id);

      if (!converted)
        return BadRequest(new { code = "InvalidId", error = "Plz provide a valid Id" });

      DepartmentModel? department = await _departmentService.GetDepartmentByIdAsync(Id);

      if (department is null)
        return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

      return Ok(department);
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while finding the department" });
    }
  }

  [HttpGet]
  [Route("GetFacultyDepartments/{faculty_id}")]
  public async Task<ActionResult<IEnumerable<FacultyModel>>> GetFacultyDepartments(string faculty_id)
  {
    if (faculty_id is null)
      return BadRequest(new { code = "EmptyId", error = "Plz provide a faculty id" });

    Guid Id;
    bool converted = Guid.TryParse(faculty_id, out Id);

    if (!converted)
      return BadRequest(new { code = "InvalidId", error = "Plz provide a valid id" });

    FacultyModel? faculty = await _facultyService.GetFacultyByIdAsync(Id);

    if (faculty is null)
      return BadRequest(new { code = "FacultyNotFound", error = "Faculty is not found" });

    return Ok(await _departmentService.GetFacultyDepartmentsAsync(faculty_id));
  }

  [HttpPut]
  [Route("UpdateHod")]
  public async Task<ActionResult<object>> UpdateDepartmentHod(UpdateDepartmentHodDto dto)
  {
    bool converted = Guid.TryParse(dto.departmentId, out Guid Id);

    if (!converted)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    DepartmentModel? department = await _departmentService.GetDepartmentByIdAsync(Id);

    if (department is null)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    UserModel hod = await _userManager.FindByIdAsync(dto.newHodId);

    if (hod is null)
      return BadRequest(new { code = "HodNotFound", error = "User is not found" });
    else if (hod.Id == dto.newHodId)
      return BadRequest(new { code = "SameHod", error = "Cannot assign the same Hod" });
    else if (hod.Role != "Hod")
      return BadRequest(new { code = "UserNotHod", error = "User is not a Hod" });

    try
    {
      _departmentService.UpdateDepartmentHod(department, hod);
      await _notificationService.CreateNotificationAsync($"Congrats, A new department '{department.Name}' has been assigned to you", hod.Id, "success");
      await _notificationService.CreateNotificationAsync($"Your department has been assigned to '{hod.FullName}'", department.HodId, "warning");
      await _notificationService.CreateNotificationAsync($"Department '{department.Name}' has been assigned to {hod.FullName}", department.CourseAdviserId, "info");
      return Ok(new { succeeded = true, department = department });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while updating department hod" });
    }
  }

  [HttpPut]
  [Route("UpdateCourseAdviser")]
  public async Task<ActionResult<object>> UpdateDepartmentCourseAdviser(UpdateDepartmentCourseAdviserDto dto)
  {
    bool couldParse = Guid.TryParse(dto.DepartmentId, out Guid Id);

    if (!couldParse)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    DepartmentModel? department = await _departmentService.GetDepartmentByIdAsync(Id);

    if (department is null)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    UserModel courseAdviser = await _userManager.FindByIdAsync(dto.NewCourseAdviserId);

    if (courseAdviser is null)
      return BadRequest(new { code = "CourseAdviserNotFound", error = "Course Adviser is not found" });
    else if (courseAdviser.Id == dto.NewCourseAdviserId)
      return BadRequest(new { code = "SameCourseAdviser", error = "Cannot assign same Course Adviser" });
    else if (courseAdviser.Role != "CourseAdviser")
      return BadRequest(new { code = "UserNotCourseAdvier", error = "User is not Course Adviser" });

    try
    {
      _departmentService.UpdateCourseAdviser(department, courseAdviser);
      await _notificationService.CreateNotificationAsync($"Congrats, A new department has been assigned to '{department.Name}'", courseAdviser.Id, "success");
      // Send notifications
      return Ok(new { succeeded = true, department = department });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while updating department hod" });
    }
  }
}
