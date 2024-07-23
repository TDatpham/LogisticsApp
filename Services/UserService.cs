using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogisticsApp.Data;
using LogisticsApp.DTOs;
using LogisticsApp.DTOs.Response;
using LogisticsApp.Repository;
using LogisticsApp.Repository.Interfaces;
using LogisticsApp.Services.Interfaces;
using LogisticsApp.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsApp.Services;

public class UserSerivce : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly LogisticsDbContext _logisticsDbContext;

    public UserSerivce(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailService emailService,
        IUserRepository userRepository, LogisticsDbContext logisticsDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _emailService = emailService;
        _userRepository = userRepository;
        _logisticsDbContext = logisticsDbContext;
    }

    public async Task<LoginResponseDto> LoginAsync(RequestLoginDTO requestLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(requestLoginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, requestLoginDto.Password))
        {
            return null;
        }

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault())
        };
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            authClaims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return new LoginResponseDto
        {
            User = user,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }

    public async Task<ApplicationUser> RegisterAsync(RequestRegisterDto requestRegisterDto)
    {
        bool userExists = await _userRepository.UserExistsAsync(requestRegisterDto.Email, requestRegisterDto.Username);
        if (userExists)
        {
            throw new Exception("User already exists");
        }

        var user = new ApplicationUser
        {
            Email = requestRegisterDto.Email,
            FullName = requestRegisterDto.Username,
            UserName = requestRegisterDto.Username,
            PhoneNumber = requestRegisterDto.PhoneNumber,
            Address = requestRegisterDto.Address,
            EmailConfirmed = false,
            PhoneNumberConfirmed = true
        };
        var registerUser = await _userRepository.CreateUserAsync(user, requestRegisterDto.Password);
        if (registerUser == null)
        {
            return null;
        }

        await _userRepository.EnsureRoleExistsAsync(user, ApplicationRoles.User);
        await _userManager.AddToRoleAsync(user, ApplicationRoles.User);
        await _emailService.SendEmailAsync(user.Email, "Welcome to LogisticsApp",
            "You have successfully registered to LogisticsApp");
        return user;
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            return null;
        }

        var otp = GenerateOTP.Generate();
        var expiration = DateTime.UtcNow.AddMinutes(15);
        await _userRepository.StoreUserOTPAsync(user.Id, otp, expiration);

        // Send OTP via email
        await _emailService.SendEmailAsync(user.Email, "Your Password Reset Code", $"Your OTP is: {otp}");

        return "OTP sent to your registered email address.";
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> VerifyOTPAsync(string userId, string otp)
    {
        var userOTP = await _logisticsDbContext.UserOTPs
            .Where(u => u.UserId == userId && u.OTP == otp && u.Expiration > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (userOTP != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logisticsDbContext.UserOTPs.Remove(userOTP);
                    await _logisticsDbContext.SaveChangesAsync();
                    return true;
                }
            }
        }

        return false;
    }

    public async Task<ResponseUserProfileDto> GetUserProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        var userProfileDto = new ResponseUserProfileDto
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address
        };
        return userProfileDto;
    }

    public async Task<ResponseUserProfileDto> UpdateUserProfileTask(string userId,
        RequestUpdateUserProfileDto requestUserProfileDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        user.Email = requestUserProfileDto.Email;
        user.PhoneNumber = requestUserProfileDto.PhoneNumber;
        user.Address = requestUserProfileDto.Address;
        await _userManager.UpdateAsync(user);
        return new ResponseUserProfileDto
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address
        };
    }

    public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword); 
    }
}