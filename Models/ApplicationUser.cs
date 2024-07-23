using LogisticsApp.Entities;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Data;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string Address { get; set; }
    public string ProfilePicUrl { get; set; }
    public virtual Driver Driver { get; set; }
}