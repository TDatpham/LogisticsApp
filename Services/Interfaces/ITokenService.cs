using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Services.Interfaces;

public interface ITokenService
{
    public string GenerateJwtToken(IdentityUser user);
}