using Microsoft.Extensions.Options;
using BusinessLayer.Interfaces;
using EntitiesLayer.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace BusinessLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string firstName)
        {
            var subject = "Hoş Geldiniz!";
            var body = $@"
        <h1>Hoş Geldiniz, {firstName}!</h1>
        <p>Uygulamamıza kaydolduğunuz için teşekkür ederiz.</p>
        <p>Herhangi bir sorunla karşılaşırsanız, lütfen bizimle iletişime geçin.</p>
        <p>İyi günler dileriz!</p>";

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
                    smtpClient.EnableSsl = true; // SSL'yi etkinleştir
                    try
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                    catch (SmtpException ex)
                    {
                        // Hata yönetimi
                        Console.WriteLine($"SMTP Hatası: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task SendMeetingNotificationEmailAsync(string toEmail, string meetingName, DateTime startDate, DateTime endDate, string description)
        {
            var subject = "Toplantı Bilgilendirmesi";
            var body = $@"
        <h1>Toplantı Bilgilendirmesi</h1>
        <p><strong>Toplantı Adı:</strong> {meetingName}</p>
        <p><strong>Başlangıç Tarihi:</strong> {startDate.ToString("dd/MM/yyyy HH:mm")}</p>
        <p><strong>Bitiş Tarihi:</strong> {endDate.ToString("dd/MM/yyyy HH:mm")}</p>
        <p><strong>Açıklama:</strong> {description}</p>
        <p>Toplantıya katılmayı unutmayın!</p>";

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
                    smtpClient.EnableSsl = true;
                    try
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                    catch (SmtpException ex)
                    {
                        Console.WriteLine($"SMTP Hatası: {ex.Message}");
                        throw;
                    }
                }
            }
        }




    }
}

