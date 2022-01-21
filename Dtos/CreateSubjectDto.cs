namespace UniversityManagementBackend.Dtos;

public class CreateSubjectDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  [Required]
  public string DepartmentId { get; set; } = string.Empty;
  [Required]
  public string TeacherId { get; set; } = string.Empty;
}