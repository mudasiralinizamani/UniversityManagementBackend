namespace UniversityManagementBackend.Dtos;

public class UpdateFacultyDeanDto
{
  [Required]
  public string FacultyId { get; set; } = string.Empty;
  [Required]
  public string NewDeanId { get; set; } = string.Empty;
}