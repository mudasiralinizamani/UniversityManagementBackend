namespace UniversityManagementBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectController : ControllerBase
{
  private readonly UserManager<UserModel> _userManager;
  private readonly ISubject _subjectService;
  private readonly IDepartment _departmentService;
  private readonly INotification _notificationService;

  public SubjectController(ISubject subjectService, IDepartment departmentService, INotification notificationService, UserManager<UserModel> userManager)
  {
    _userManager = userManager;
    _subjectService = subjectService;
    _departmentService = departmentService;
    _notificationService = notificationService;
  }

  [HttpPost]
  [Route("Create")]
  public async Task<ActionResult<object>> CreateSubject(CreateSubjectDto dto)
  {

    bool converted = Guid.TryParse(dto.DepartmentId, out Guid Id);

    if (!converted)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    DepartmentModel? department = await _departmentService.GetDepartmentByIdAsync(Id);

    if (department is null)
      return BadRequest(new { code = "DepartmentNotFound", error = "Department is not found" });

    UserModel teacher = await _userManager.FindByIdAsync(dto.TeacherId);

    if (teacher is null)
      return BadRequest(new { code = "TeacherNotFound", error = "Teacher is not found" });
    else if (teacher.Role != "Teacher")
      return BadRequest(new { code = "UserNotTeacher", error = "User is not teacher" });

    SubjectModel? subject = await _subjectService.GetSubjectByNameAsync(dto.Name);

    if (subject is not null)
      return BadRequest(new { code = "SubjectNameFound", error = "Subject with same name already exists" });

    try
    {
      SubjectModel model = new()
      {
        CourseAdviserId = department.CourseAdviserId,
        CourseAdviserName = department.CourseAdviserName,
        DepartmentId = department.Id.ToString(),
        DepartmentName = department.Name,
        Id = Guid.NewGuid(),
        Name = dto.Name,
        TeacherId = teacher.Id,
        TeacherName = teacher.FullName,
      };

      await _subjectService.CreateSubjectAsync(model);
      await _notificationService.CreateNotificationAsync($"Congrats, A new subject has been assigned to you", department.CourseAdviserId, "success");
      await _notificationService.CreateNotificationAsync($"A new subject '{dto.Name}' has been added your department '{department.Name}'", department.HodId, "info");
      await _notificationService.CreateNotificationAsync($"Congrats, A new subject has been assigned to you", teacher.Id, "success");

      return Ok(new { succeeded = true });
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while creating department" });
    }
  }

  [HttpGet]
  [Route("GetSubjects")]
  public async Task<ActionResult<IEnumerable<SubjectModel>>> GetSubjects()
  {
    return Ok(await _subjectService.GetSubjectsAsync());
  }

  [HttpGet]
  [Route("GetSubject/{subject_id}")]
  public async Task<ActionResult<SubjectModel>> GetSubject(string subject_id)
  {
    Guid Id;
    bool converted = Guid.TryParse(subject_id, out Id);

    if (!converted)
      return BadRequest(new { code = "SubjectNotFound", error = "Subject is not found" });

    try
    {
      SubjectModel? subject = await _subjectService.GetSubjectByIdAsync(Id);

      if (subject is null)
        return BadRequest(new { code = "SubjectNotFound", error = "Subject is not found" });

      return Ok(subject);
    }
    catch (Exception)
    {
      return BadRequest(new { code = "ServerError", error = "Error occurred while finding the department" });
    }
  }
}