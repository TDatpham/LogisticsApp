using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.DTOs;

public class RequestChangePasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}