using Depom.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Depom.Application.User.DTOs;

public class UserCreateDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "KullanÄ±cÄ± adÄ± zorunludur.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "KullanÄ±cÄ± adÄ± 3-50 karakter arasÄ±nda olmalÄ±dÄ±r.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Åifre zorunludur.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Åifre en az 6 karakter olmalÄ±dÄ±r.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol seÃ§imi zorunludur.")]
    public AppRole Role { get; set; } = AppRole.Viewer;

    public int? BranchId { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UserUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "KullanÄ±cÄ± adÄ± zorunludur.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "KullanÄ±cÄ± adÄ± 3-50 karakter arasÄ±nda olmalÄ±dÄ±r.")]
    public string Username { get; set; } = string.Empty;

    /// <summary>BoÅŸ bÄ±rakÄ±lÄ±rsa ÅŸifre deÄŸiÅŸtirilmez.</summary>
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Åifre en az 6 karakter olmalÄ±dÄ±r.")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "Rol seÃ§imi zorunludur.")]
    public AppRole Role { get; set; }

    public int? BranchId { get; set; }

    public bool IsActive { get; set; }
}

public class UserListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public AppRole Role { get; set; }
    public string RoleDisplay => Role switch
    {
        AppRole.SystemAdmin   => "Sistem YÃ¶neticisi",
        AppRole.BranchManager => "Åube MÃ¼dÃ¼rÃ¼",
        AppRole.StockClerk    => "Depo Personeli",
        AppRole.Viewer        => "Ä°zleyici",
        _                     => Role.ToString()
    };
    public string? BranchName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class UserDetailDto : UserListDto
{
    public int? BranchId { get; set; }
}