using LogisticsApp.Data;
using LogisticsApp.DTOs;
using LogisticsApp.DTOs.Response;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Services.Interfaces;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(RequestLoginDTO requestLoginDto);
    Task<ApplicationUser> RegisterAsync(RequestRegisterDto requestRegisterDto);
    Task<string> ForgotPasswordAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<bool> VerifyOTPAsync(string userId, string otp);
    Task<ResponseUserProfileDto> GetUserProfileAsync(string userId);
    Task<ResponseUserProfileDto> UpdateUserProfileTask(string userId, RequestUpdateUserProfileDto requestUserProfileDto);
    Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword); 
}