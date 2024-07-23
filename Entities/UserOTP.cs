using LogisticsApp.Data;

namespace LogisticsApp.Entities;

public class UserOTP
{
    
    public int Id { get; set; }
    public string UserId { get; set; }  
    public string OTP { get; set; }
    public DateTime Expiration { get; set; }
    public ApplicationUser User { get; set; }  
}