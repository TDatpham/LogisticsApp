namespace LogisticsApp.DTOs;

public class RequestResetPasswordDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}