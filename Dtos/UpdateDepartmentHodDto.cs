namespace UniversityManagementBackend.Dtos;

public class UpdateDepartmentHodDto
{
  [Required]
  public string departmentId { get; set; } = string.Empty;
  [Required]
  public string newHodId { get; set; } = string.Empty;
}