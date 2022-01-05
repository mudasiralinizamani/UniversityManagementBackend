namespace UniversityManagementBackend.Dtos;

public class SignupDto
{
  [Required]
  [MinLength(3)]
  [MaxLength(30)]
  public string FullName { get; set; } = string.Empty;
  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;
  [Required]
  public string Password { get; set; } = string.Empty;
  [Required]
  public string Role { get; set; } = string.Empty;
  [Required]
  public string ProfilePic { get; set; } = string.Empty;
}
