using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UniversityManagementBackend.Data;

public class AuthContext : IdentityDbContext
{
  public AuthContext(DbContextOptions<AuthContext> opts) : base(opts) { }

  public DbSet<UserModel> Users { get; set; }

}