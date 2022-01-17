namespace UniversityManagementBackend.Models;

public class StudentSubjectsModel
{
  public Guid Id { get; set; }
  public string SubjectId { get; set; } = string.Empty;
  public string StudentId { get; set; } = string.Empty;
}