namespace UniversityManagementBackend.Interfaces;

public interface IDepartment
{
  Task CreateDepartmentAsync(DepartmentModel model);
  Task<IEnumerable<DepartmentModel>> GetDepartmentsAsync();
  Task<DepartmentModel?> GetDepartmentByIdAsync(Guid id);
  void DeleteDepartment(DepartmentModel model);
  Task<DepartmentModel?> GetDepartmentByNameAsync(string name);
  Task<IEnumerable<DepartmentModel>> GetFacultyDepartmentsAsync(string faculty_id);
  DepartmentModel UpdateFacultyName(DepartmentModel department, string faculty_name);
  DepartmentModel UpdateDepartmentHod(DepartmentModel department, UserModel hod);
  DepartmentModel UpdateCourseAdviser(DepartmentModel department, UserModel courseAdviser);
}