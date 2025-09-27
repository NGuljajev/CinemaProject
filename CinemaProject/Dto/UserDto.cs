using System.ComponentModel.DataAnnotations;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public int? RoleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
}

public class CreateUserDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; } // plain text only for creation, hash it before saving

    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public int? RoleId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateUserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; } // optional, only update if provided
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public int? RoleId { get; set; }
    public bool? IsActive { get; set; }
}
