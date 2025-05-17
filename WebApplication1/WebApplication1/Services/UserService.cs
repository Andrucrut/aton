using System;
using System.Collections.Concurrent;
using UserManagementApi.DTOs;
using WebApplication1.DTOs;
using WebApplication1.Interfaces;
using WebApplication1.Models;


namespace WebApplication1.Services;

public class UserService : IUserService
{
    private readonly ConcurrentDictionary<string, User> _users = new();
    
    public UserService()
    {
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Login = "admin",
            Password = "admin123",
            Name = "Admin",
            Gender = 1,
            Birthday = new DateTime(1990, 1, 1),
            Admin = true,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "system",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "system"
        };
        _users.TryAdd(admin.Login.ToLower(), admin);
    }

    public Task<User> CreateUserAsync(UserCreateDto dto, string createdBy)
    {
        if (_users.ContainsKey(dto.Login.ToLower()))
            throw new InvalidOperationException("Login already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = dto.Login,
            Password = dto.Password,
            Name = dto.Name,
            Gender = dto.Gender,
            Birthday = dto.Birthday,
            Admin = dto.Admin,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createdBy,
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = createdBy
        };

        _users.TryAdd(dto.Login.ToLower(), user);
        return Task.FromResult(user);
    }

    public Task<bool> UpdateUserInfoAsync(string login, UserUpdateDto dto, string modifiedBy)
    {
        if (!_users.TryGetValue(login.ToLower(), out var user)) return Task.FromResult(false);

        user.Name = dto.Name ?? user.Name;
        user.Gender = dto.Gender ?? user.Gender;
        user.Birthday = dto.Birthday ?? user.Birthday;
        user.ModifiedBy = modifiedBy;
        user.ModifiedOn = DateTime.UtcNow;

        return Task.FromResult(true);
    }


    public Task<bool> ChangePasswordAsync(string login, string newPassword, string modifiedBy)
    {
        if (!_users.TryGetValue(login.ToLower(), out var user)) return Task.FromResult(false);

        user.Password = newPassword;
        user.ModifiedBy = modifiedBy;
        user.ModifiedOn = DateTime.UtcNow;

        return Task.FromResult(true);
    }

    public Task<bool> ChangeLoginAsync(string oldLogin, string newLogin, string modifiedBy)
    {
        if (!_users.TryGetValue(oldLogin.ToLower(), out var user)) return Task.FromResult(false);
        if (_users.ContainsKey(newLogin.ToLower())) return Task.FromResult(false);

        _users.TryRemove(oldLogin.ToLower(), out _);

        user.Login = newLogin;
        user.ModifiedBy = modifiedBy;
        user.ModifiedOn = DateTime.UtcNow;

        _users.TryAdd(newLogin.ToLower(), user);

        return Task.FromResult(true);
    }

    public Task<List<User>> GetActiveUsersAsync()
    {
        var activeUsers = _users.Values
            .Where(u => u.RevokedOn == null)
            .OrderBy(u => u.CreatedOn)
            .ToList();

        return Task.FromResult(activeUsers);
    }

    public Task<UserViewDto?> GetUserInfoByLoginAsync(string login)
    {
        if (!_users.TryGetValue(login.ToLower(), out var user)) return Task.FromResult<UserViewDto?>(null);

        var dto = new UserViewDto
        {
            Name = user.Name,
            Gender = user.Gender,
            Birthday = user.Birthday,
            IsActive = user.RevokedOn == null
        };

        return Task.FromResult<UserViewDto?>(dto);
    }

    public Task<User?> GetUserByCredentialsAsync(string login, string password)
    {
        if (!_users.TryGetValue(login.ToLower(), out var user)) return Task.FromResult<User?>(null);

        if (user.Password != password || user.RevokedOn != null)
            return Task.FromResult<User?>(null);

        return Task.FromResult<User?>(user);
    }

    public Task<List<User>> GetUsersOlderThanAsync(int age)
    {
        var today = DateTime.Today;
        var result = _users.Values
            .Where(u => u.Birthday.HasValue &&
                        (today.Year - u.Birthday.Value.Year -
                        (today < u.Birthday.Value.AddYears(today.Year - u.Birthday.Value.Year) ? 1 : 0)) > age)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<bool> SoftDeleteUserAsync(string login, string revokedBy)
    {
        if (!_users.TryGetValue(login.ToLower(), out var user)) return Task.FromResult(false);

        user.RevokedOn = DateTime.UtcNow;
        user.RevokedBy = revokedBy;

        return Task.FromResult(true);
    }

    public Task<bool> HardDeleteUserAsync(string login)
    {
        return Task.FromResult(_users.TryRemove(login.ToLower(), out _));
    }

    public Task<bool> RestoreUserAsync(string login)
    {
        if (!_users.TryGetValue(login.ToLower(), out var user)) return Task.FromResult(false);

        user.RevokedOn = null;
        user.RevokedBy = null;

        return Task.FromResult(true);
    }
}
