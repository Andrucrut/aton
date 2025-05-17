using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class UserCreateDto
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$")]
    public string Login { get; set; } = null!;

    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$")]
    public string Password { get; set; } = null!;

    [Required]
    [RegularExpression("^[a-zA-Zа-яА-Я]+$")]
    public string Name { get; set; } = null!;

    [Range(0, 2)]
    public int Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public bool Admin { get; set; }
}