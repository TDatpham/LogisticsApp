using System.Net;
using System.Net.Mail;
using LogisticsApp.Services.Interfaces;

namespace LogisticsApp.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient(_configuration["EmailSettings:Host"])
        {
            Port = int.Parse(_configuration["EmailSettings:Port"]),
            Credentials = new NetworkCredential(_configuration["EmailSettings:User"],
                _configuration["EmailSettings:Password"]),
            EnableSsl = true
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:User"]),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);
        await smtpClient.SendMailAsync(mailMessage);
    }
}