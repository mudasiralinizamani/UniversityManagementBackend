namespace UniversityManagementBackend.Services;

public class IFacultyService : IFaculty
{
  private readonly ApiContext _context;

  public IFacultyService(ApiContext context)
  {
    _context = context;
  }
  public async Task CreateFacultyAsync(FacultyModel model)
  {
    ArgumentNullException.ThrowIfNull(model);
    await _context.faculties.AddAsync(model);
    await _context.SaveChangesAsync();
  }

  public void DeleteFaculty(FacultyModel model)
  {
    ArgumentNullException.ThrowIfNull(model);
    _context.faculties.Remove(model);
    _context.SaveChanges();
  }

  public async Task<IEnumerable<FacultyModel>> GetFacultiesAsync()
  {
    return (await _context.faculties.ToListAsync());
  }

  public async Task<FacultyModel?> GetFacultyByIdAsync(Guid id)
  {
    ArgumentNullException.ThrowIfNull(id);
    return (await _context.faculties.FindAsync(id));
  }

  public async Task<FacultyModel?> GetFacultyByNameAsync(string name)
  {
    ArgumentNullException.ThrowIfNull(name);
    return (await _context.faculties.FirstOrDefaultAsync(x => x.Name == name));
  }

  public FacultyModel UpdateFacultyDean(FacultyModel faculty, UserModel dean)
  {
    ArgumentNullException.ThrowIfNull(faculty);
    ArgumentNullException.ThrowIfNull(dean);

    faculty.DeanId = dean.Id;
    faculty.DeanName = dean.FullName;

    return faculty;
  }

  public FacultyModel UpdateFacultyName(FacultyModel faculty, string name)
  {
    ArgumentNullException.ThrowIfNull(faculty);
    ArgumentNullException.ThrowIfNull(name);

    faculty.Name = name;

    return faculty;
  }
}