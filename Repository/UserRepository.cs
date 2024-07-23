using Azure.Core;
using LogisticsApp.Data;
using LogisticsApp.DTOs;
using LogisticsApp.Entities;
using LogisticsApp.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly LogisticsDbContext _logisticsDbContext; 
    public UserRepository(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, 
         LogisticsDbContext logisticsDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logisticsDbContext = logisticsDbContext;
    }
    public async Task<ApplicationUser> LoginAsync(RequestLoginDTO requestLoginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(requestLoginDto.Email, requestLoginDto.Password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(requestLoginDto.Email);
            return user;
        }

        return null;
    }
    
    public async Task<ApplicationUser> RegisterAsync(RequestRegisterDto requestRegisterDto)
    {
        var user = new ApplicationUser
        {
            UserName = requestRegisterDto.Email,
            Email = requestRegisterDto.Email,
            PhoneNumberConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, requestRegisterDto.Password);

        if (result.Succeeded)
        {
            return user;
        }

        return null;
    }

    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return user; 
        }
        return null;
    }
    public async Task AddToRoleAsync(ApplicationUser user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName); 
    }

    public async Task<bool> EnsureRoleExistsAsync(ApplicationUser user, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return true;
        }

        return false; 
    }

    public async Task<bool> UserExistsAsync(string email, string userName)
    {
        var userByEmail = await _userManager.FindByEmailAsync(email);
        var userByUserName = await _userManager.FindByNameAsync(userName); 
        return userByEmail != null || userByUserName != null;

    }
    
    public async Task<ApplicationUser> ValidateUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email); 
        if(user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return user; 
        }

        return null; 
    }

    public async Task<bool> StoreUserOTPAsync(string userId, string otp, DateTime expiration)
    {
        var existingOTP = await _logisticsDbContext.UserOTPs.FirstOrDefaultAsync(u => u.UserId == userId);
        if (existingOTP != null)
        {
            existingOTP.OTP = otp;
            existingOTP.Expiration = expiration;
            _logisticsDbContext.UserOTPs.Update(existingOTP);
        }
        else
        {
            var userOTP = new UserOTP
            {
                UserId = userId,
                OTP = otp,
                Expiration = expiration
            };
            
            await _logisticsDbContext.UserOTPs.AddAsync(userOTP);
        }

        var result = await _logisticsDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<IdentityUser> FindByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}