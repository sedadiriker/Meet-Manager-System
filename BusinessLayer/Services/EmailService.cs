using System.Net;
using System.Net.Mail;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpServer = _configuration["SmtpSettings:Server"];
        _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        _smtpUser = _configuration["SmtpSettings:Username"];
        _smtpPassword = _configuration["SmtpSettings:Password"];
        _senderEmail = _configuration["SmtpSettings:SenderEmail"];
        _senderName = _configuration["SmtpSettings:SenderName"];
    }

    public async Task SendWelcomeEmailAsync(string email, string firstName)
    {
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_senderEmail, _senderName),
            Subject = "Hoş Geldiniz!",
            Body = $"Merhaba {firstName},<br/><br/>Kayıt işleminiz başarılı!<br/><br/>Teşekkürler,<br/>YourAppName",
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log exception details here
            Console.WriteLine($"E-posta gönderimi sırasında bir hata oluştu: {ex.Message}");
            throw;
        }
    }
}
}
