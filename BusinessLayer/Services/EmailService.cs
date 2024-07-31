using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using EntitiesLayer.Models;

namespace BusinessLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendMeetingNotificationAsync(string toEmail, Meeting meeting)
        {
            var subject = "Toplantı Bilgilendirmesi";
            var body = $@"
            <html>
            <body>
                 <div>
            <h1>Toplantı Bilgilendirmesi</h1>
            <p>Merhaba,</p>
            <p>Bu e-posta, yaklaşan toplantınız hakkında sizi bilgilendirmek için gönderilmiştir. Toplantı detayları aşağıda yer almaktadır:</p>
            
            <div>
                <p><strong>Toplantı Adı:</strong> {meeting.Name}</p>
                <p><strong>Başlangıç Tarihi:</strong> {meeting.StartDate.ToString("dd/MM/yyyy HH:mm")}</p>
                <p><strong>Bitiş Tarihi:</strong> {meeting.EndDate.ToString("dd/MM/yyyy HH:mm")}</p>
                <p><strong>Açıklama:</strong> {meeting.Description}</p>
            </div>
            
            <p>Toplantıya katılmayı unutmayın! Size en iyi deneyimi sunmak için buradayız.</p>
            
            <p>Herhangi bir sorunuz veya yardım ihtiyacınız varsa, lütfen bizimle iletişime geçmekten çekinmeyin.</p>
            
            <div>
                <p>Bu e-posta, toplantı bilgilendirmesi olarak gönderilmiştir. Bilgilerinizi güncel tutmak için lütfen uygulamanızı kontrol edin.</p>
            </div>
            
            <p>Saygılarımızla,<br> Meet Manager Ekibi</p>
        </div>
            </body>
            </html>";

            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer))
            {
                smtpClient.Port = _emailSettings.SmtpPort;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"SMTP Hatası: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string firstName)
        {
            var subject = "Hoş Geldiniz!";
            var body = $@"
            <html>
            <body>
                <h1>Hoş Geldiniz, {firstName}!</h1>
                <p>Uygulamamıza kaydolduğunuz için teşekkür ederiz.</p>
                <p>Herhangi bir sorunla karşılaşırsanız, lütfen bizimle iletişime geçin.</p>
                <p>İyi günler dileriz!</p>
            </body>
            </html>";

            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer))
            {
                smtpClient.Port = _emailSettings.SmtpPort;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
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
