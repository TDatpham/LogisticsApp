using LogisticsApp.Data;

namespace LogisticsApp.DTOs.Response;

public class LoginResponseDto
{
    public ApplicationUser User { get; set; }
    public string Token { get; set; }
}