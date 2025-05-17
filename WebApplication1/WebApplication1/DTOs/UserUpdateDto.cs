using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class UserUpdateDto
{
    [RegularExpression("^[a-zA-Zа-яА-Я]+$")]
    public string? Name { get; set; }

    [Range(0, 2)]
    public int? Gender { get; set; }

    public DateTime? Birthday { get; set; }
}