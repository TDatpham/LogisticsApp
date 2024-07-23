using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.DTOs;

public class RequestForgotPasswordDto
{
    [Required]
    public string Email { get; set; }
}