namespace UniversityManagementBackend.Models;

public class SigninDto
{
  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;
  [Required]
  public string Password { get; set; } = string.Empty;
}