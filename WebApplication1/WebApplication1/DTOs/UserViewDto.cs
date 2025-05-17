namespace UserManagementApi.DTOs;

public class UserViewDto
{
    public string Name { get; set; } = null!;
    public int Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public bool IsActive { get; set; }
}