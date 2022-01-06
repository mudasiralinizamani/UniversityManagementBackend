namespace UniversityManagementBackend.Services;

public class IFacultyService : IFaculty
{
  private readonly ApiContext _context;

  public IFacultyService(ApiContext context)
  {
    _context = context;
  }
  public async Task CreateFacultyAsync(FacultyModel? model)
  {
    if (model is null)
      throw new ArgumentNullException();

    await _context.faculties.AddAsync(model);
    await _context.SaveChangesAsync();
  }

  public void DeleteFaculty(FacultyModel? model)
  {
    if (model is null)
      throw new ArgumentNullException();

    _context.faculties.Remove(model);
    _context.SaveChanges();
  }

  public async Task<IEnumerable<FacultyModel>> GetFacultiesAsync()
  {
    return (await _context.faculties.ToListAsync());
  }

  public async Task<FacultyModel?> GetFacultyByIdAsync(Guid? id)
  {
    if (id is null)
      throw new ArgumentNullException();

    return (await _context.faculties.FindAsync(id));
  }

  public async Task<FacultyModel?> GetFacultyByNameAsync(string? name)
  {
    if (name is null)
      throw new ArgumentNullException();

    return (await _context.faculties.FirstOrDefaultAsync(x => x.Name == name));
  }
}