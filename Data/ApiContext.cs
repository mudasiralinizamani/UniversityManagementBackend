namespace UniversityManagementBackend.Data;

public class ApiContext : DbContext
{
  public ApiContext(DbContextOptions<ApiContext> opts) : base(opts) { }

  public DbSet<FacultyModel> faculties { get; set; }
  public DbSet<DepartmentModel> departments { get; set; }
  public DbSet<NotificationModel> notifications { get; set; }
}