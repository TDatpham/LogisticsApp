using System.Security.Claims;
using Azure.Core;
using LogisticsApp.Data;
using LogisticsApp.DTOs;
using LogisticsApp.Repository.Interfaces;
using LogisticsApp.Services.Interfaces;
using LogisticsApp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsApp.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IUserService userService, IUserRepository userRepository, IEmailService emailService,
        UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _userRepository = userRepository;
        _emailService = emailService;
        _userManager = userManager;
    }

    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUserTask([FromBody] RequestLoginDTO requestLoginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.LoginAsync(requestLoginDto);
            if (response == null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("/register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUserTask([FromBody] RequestRegisterDto requestRegisterDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.RegisterAsync(requestRegisterDto);

            if (response == null)
            {
                return BadRequest("Failed to register user");
            }

            var otp = GenerateOTP.Generate();
            var expiration = DateTime.UtcNow.AddMinutes(15);
            bool otpStored = await _userRepository.StoreUserOTPAsync(response.Id, otp, expiration);
            if (!otpStored)
            {
                return BadRequest("Failed to store verification code.");
            }

            var emailContent = $"Your verification code is: {otp}. It expires in 15 minutes.";
            await _emailService.SendEmailAsync(requestRegisterDto.Email, "Verify Your Email", emailContent);
            return Ok(new
            {
                message = "User registered successfully. Please check your email to verify your account."
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("/forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordTask(RequestForgotPasswordDto requestForgotPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.ForgotPasswordAsync(requestForgotPasswordDto.Email);

            if (response == null)
            {
                return BadRequest("Failed to send password reset email");
            }

            return Ok(new { Message = response });
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("/verify-otp")]
    public async Task<IActionResult> VerifyOTPTask([FromBody] RequestVerifyOTP requestVerifyOtp)
    {
        try
        {
            var result = await _userService.VerifyOTPAsync(requestVerifyOtp.UserId, requestVerifyOtp.OTP);
            if (!result)
            {
                return BadRequest("Invalid OTP");
            }

            return Ok("OTP verified successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordTask([FromBody] RequestResetPasswordDto requestResetPasswordDto)
    {
        var result = await _userService.ResetPasswordAsync(requestResetPasswordDto.Email, requestResetPasswordDto.Token,
            requestResetPasswordDto.NewPassword);
        if (!result)
        {
            return BadRequest("Password reset failed.");
        }

        return Ok("Password reset successfully.");
    }

    [HttpGet("get-profile-user")]
    [Authorize(Roles = ApplicationRoles.User)]
    public async Task<IActionResult> GetUserProfileTask()
    {
        try
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing from the token");
            }

            var userProfileDto = await _userService.GetUserProfileAsync(userId);
            if (userProfileDto == null)
            {
                return NotFound("User not found");
            }

            return Ok(userProfileDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("update-profile")]
    [Authorize(Roles = ApplicationRoles.User + "," + ApplicationRoles.Admin)]
    public async Task<IActionResult> UpdateUserProfileTask([FromBody] RequestUpdateUserProfileDto userProfileDto)
    {
        try
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing from the token");
            }

            var response = await _userService.UpdateUserProfileTask(userId, userProfileDto);
            if (response == null)
            {
                return BadRequest("Failed to update user profile");
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost("change-password")]
    [Authorize(Roles = ApplicationRoles.User)]
    public async Task<IActionResult> ChangePasswordTask([FromBody] RequestChangePasswordDto requestChangePasswordDto)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new
                {
                    message = "User not found"
                });
            }

            var result = await _userService.ChangePasswordAsync(user, requestChangePasswordDto.CurrentPassword,
                requestChangePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description).ToList());
            }
            return Ok(new
            {
                message = "Password changed successfully."
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }
}