using Microsoft.AspNetCore.Server.HttpSys;

namespace LogisticsApp.DTOs;

public class RequestUpdateUserProfileDto
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}