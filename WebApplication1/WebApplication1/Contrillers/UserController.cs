using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
    {
        var createdBy = "system"; 
        try
        {
            var user = await _userService.CreateUserAsync(dto, createdBy);
            return CreatedAtAction(nameof(GetUserInfo), new { login = user.Login }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveUsers()
    {
        var users = await _userService.GetActiveUsersAsync();
        return Ok(users);
    }

    [HttpGet("{login}")]
    public async Task<IActionResult> GetUserInfo(string login)
    {
        var userDto = await _userService.GetUserInfoByLoginAsync(login);
        if (userDto == null) return NotFound();
        return Ok(userDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var user = await _userService.GetUserByCredentialsAsync(dto.Login, dto.Password);
        if (user == null) return Unauthorized();
        return Ok(user);
    }

    [HttpPut("{login}")]
    public async Task<IActionResult> UpdateUser(string login, [FromBody] UserUpdateDto dto)
    {
        var modifiedBy = "system"; // или взять из авторизации
        var result = await _userService.UpdateUserInfoAsync(login, dto, modifiedBy);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPut("{login}/password")]
    public async Task<IActionResult> ChangePassword(string login, [FromBody] UserPasswordChangeDto dto)
    {
        var modifiedBy = "system";
        var result = await _userService.ChangePasswordAsync(login, dto.NewPassword, modifiedBy);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPut("{login}/login")]
    public async Task<IActionResult> ChangeLogin(string login, [FromBody] UserLoginChangeDto dto)
    {
        var modifiedBy = "system";
        var result = await _userService.ChangeLoginAsync(login, dto.NewLogin, modifiedBy);
        if (!result) return Conflict("New login already in use or user not found");
        return NoContent();
    }

    [HttpGet("older-than/{age}")]
    public async Task<IActionResult> GetUsersOlderThan(int age)
    {
        var users = await _userService.GetUsersOlderThanAsync(age);
        return Ok(users);
    }

    [HttpDelete("{login}/soft")]
    public async Task<IActionResult> SoftDelete(string login)
    {
        var revokedBy = "system";
        var result = await _userService.SoftDeleteUserAsync(login, revokedBy);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{login}/hard")]
    public async Task<IActionResult> HardDelete(string login)
    {
        var result = await _userService.HardDeleteUserAsync(login);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{login}/restore")]
    public async Task<IActionResult> Restore(string login)
    {
        var result = await _userService.RestoreUserAsync(login);
        if (!result) return NotFound();
        return NoContent();
    }
}
