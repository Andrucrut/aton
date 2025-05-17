using System.ComponentModel.DataAnnotations;
namespace WebApplication1.DTOs;

public class UserPasswordChangeDto
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$")]
    public string NewPassword { get; set; } = null!;
}