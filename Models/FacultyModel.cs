namespace UniversityManagementBackend.Models;

public class FacultyModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string DeanId { get; set; } = string.Empty;
  public string DeanName { get; set; } = string.Empty;
}