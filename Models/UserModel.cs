namespace UniversityManagementBackend.Models;

public class UserModel : IdentityUser
{
  public string FullName { get; set; } = string.Empty;
  public string ProfilePic { get; set; } = string.Empty;
  public string Role { get; set; } = string.Empty;
}