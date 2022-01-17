namespace UniversityManagementBackend.Models;

public class SubjectModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string DepartmentId { get; set; } = string.Empty;
  public string DepartmentName { get; set; } = string.Empty;
  public string CourseAdviserId { get; set; } = string.Empty;
  public string CourseAdviserName { get; set; } = string.Empty;
}