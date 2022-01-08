namespace UniversityManagementBackend.Models;

public class DepartmentModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string HodId { get; set; } = string.Empty;
  public string HodName { get; set; } = string.Empty;
  public string CourseAdviserId { get; set; } = string.Empty;
  public string CourseAdviserName { get; set; } = string.Empty;
  public string FacultyId { get; set; } = string.Empty;
  public string FacultyName { get; set; } = string.Empty;
}