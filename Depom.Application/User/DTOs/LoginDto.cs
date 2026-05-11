using System.ComponentModel.DataAnnotations;

namespace Depom.Application.User.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Kullanici adi zorunludur.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sifre zorunludur.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
