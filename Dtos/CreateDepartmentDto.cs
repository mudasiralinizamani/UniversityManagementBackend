namespace UniversityManagementBackend.Dtos;

public class CreateDepartmentDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  [Required]
  public string HodId { get; set; } = string.Empty;
  [Required]
  public string CourseAdviserId { get; set; } = string.Empty;
  [Required]
  public Guid FacultyId { get; set; }
}