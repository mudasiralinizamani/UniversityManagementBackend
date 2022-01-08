namespace UniversityManagementBackend.Interfaces;

public interface IDepartment
{
  Task CreateDepartmentAsync(DepartmentModel model);
  Task<IEnumerable<DepartmentModel>> GetDepartmentsAsync();
  Task<DepartmentModel?> GetDepartmentByIdAsync(Guid id);
  void DeleteDepartment(DepartmentModel model);
  Task<DepartmentModel?> GetDepartmentByNameAsync(string name);
}