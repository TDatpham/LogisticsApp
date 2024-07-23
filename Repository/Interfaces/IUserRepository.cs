using LogisticsApp.Data;
using LogisticsApp.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Repository.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser> LoginAsync(RequestLoginDTO requestLoginDto);
    Task<ApplicationUser> RegisterAsync(RequestRegisterDto requestRegisterDto);
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
    Task AddToRoleAsync(ApplicationUser user, string role);
    Task<bool> EnsureRoleExistsAsync(ApplicationUser user, string role);
    Task<bool> UserExistsAsync(string email, string userName);
    Task<ApplicationUser> ValidateUserAsync(string email, string password);
    Task<bool> StoreUserOTPAsync(string userId, string otp, DateTime expiration);
    Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
    Task<IdentityUser> FindByIdAsync(string userId);
}