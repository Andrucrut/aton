using UserManagementApi.DTOs;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(UserCreateDto userCreateDto, string createdBy);
    
    Task<bool> UpdateUserInfoAsync(string login, UserUpdateDto userUpdateDto, string modifiedBy);
    Task<bool> ChangePasswordAsync(string login, string newPassword, string modifiedBy);
    Task<bool> ChangeLoginAsync(string oldLogin, string newLogin, string modifiedBy);
    
    Task<List<User>> GetActiveUsersAsync();
    Task<UserViewDto?> GetUserInfoByLoginAsync(string login);
    Task<User?> GetUserByCredentialsAsync(string login, string password);
    Task<List<User>> GetUsersOlderThanAsync(int age);
    
    Task<bool> SoftDeleteUserAsync(string login, string revokedBy);
    Task<bool> HardDeleteUserAsync(string login);
    Task<bool> RestoreUserAsync(string login);
}
