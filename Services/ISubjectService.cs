namespace UniversityManagementBackend.Services;

public class ISubjectService : ISubject
{
  private readonly ApiContext _context;

  public ISubjectService(ApiContext context)
  {
    _context = context;
  }
  public async Task CreateSubjectAsync(SubjectModel model)
  {
    ArgumentNullException.ThrowIfNull(model);
    await _context.subjects.AddAsync(model);
    await _context.SaveChangesAsync();
  }

  public void DeleteSubject(SubjectModel subject)
  {
    ArgumentNullException.ThrowIfNull(subject);
    _context.subjects.Remove(subject);
    _context.SaveChanges();
  }

  public async Task<SubjectModel?> GetSubjectByIdAsync(Guid id)
  {
    ArgumentNullException.ThrowIfNull(id);
    return (await _context.subjects.FirstOrDefaultAsync(x => x.Id == id));
  }

  public async Task<SubjectModel?> GetSubjectByNameAsync(string name)
  {
    ArgumentNullException.ThrowIfNull(name);
    return (await _context.subjects.FirstOrDefaultAsync(x => x.Name == name));
  }

  public async Task<IEnumerable<SubjectModel>> GetSubjectsAsync()
  {
    return (await _context.subjects.ToListAsync());
  }
}