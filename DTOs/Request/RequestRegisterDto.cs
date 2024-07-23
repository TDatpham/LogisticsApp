using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.DTOs;

public class RequestRegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string confirmPassword { get; set; }
    public string Address { get; set; }
}