namespace UniversityManagementBackend.Services;

public class IDepartmentService : IDepartment
{
  private readonly ApiContext _context;

  public IDepartmentService(ApiContext context)
  {
    _context = context;
  }
  public async Task CreateDepartmentAsync(DepartmentModel model)
  {
    ArgumentNullException.ThrowIfNull(model);
    await _context.departments.AddAsync(model);
    await _context.SaveChangesAsync();
  }

  public void DeleteDepartment(DepartmentModel model)
  {
    ArgumentNullException.ThrowIfNull(model);
    _context.departments.Remove(model);
    _context.SaveChanges();
  }

  public async Task<DepartmentModel?> GetDepartmentByIdAsync(Guid id)
  {
    ArgumentNullException.ThrowIfNull(id);
    return (await _context.departments.FirstOrDefaultAsync(x => x.Id == id));
  }

  public async Task<DepartmentModel?> GetDepartmentByNameAsync(string name)
  {
    ArgumentNullException.ThrowIfNull(name);
    return (await _context.departments.FirstOrDefaultAsync(x => x.Name == name));
  }

  public async Task<IEnumerable<DepartmentModel>> GetDepartmentsAsync()
  {
    return (await _context.departments.ToListAsync());
  }

  public async Task<IEnumerable<DepartmentModel>> GetFacultyDepartmentsAsync(string faculty_id)
  {
    ArgumentNullException.ThrowIfNull(faculty_id);

    return (await _context.departments.Where(x => x.FacultyId == faculty_id).ToListAsync());
  }

  public DepartmentModel UpdateFacultyName(DepartmentModel department, string faculty_name)
  {
    department.FacultyName = faculty_name;
    return department;
  }
}