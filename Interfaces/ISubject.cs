namespace UniversityManagementBackend.Interfaces;

public interface ISubject
{
  Task CreateSubjectAsync(SubjectModel model);
  Task<IEnumerable<SubjectModel>> GetSubjectsAsync();
  Task<SubjectModel?> GetSubjectByIdAsync(Guid id);
  void DeleteSubject(SubjectModel subject);
  Task<SubjectModel?> GetSubjectByNameAsync(string name);
}