namespace UniversityManagementBackend.Interfaces;

public interface IFaculty
{
  Task CreateFacultyAsync(FacultyModel? model);
  Task<FacultyModel?> GetFacultyByNameAsync(string? name);
  Task<IEnumerable<FacultyModel>> GetFacultiesAsync();
  Task<FacultyModel?> GetFacultyByIdAsync(Guid? id);
  void DeleteFaculty(FacultyModel? model);
}